/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 29.01.2010
 * Time: 21:09
 *  
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace OneCService2
{
	/*Контейнер для хранения соединения (адаптера) в пуле*/
	public class PoolableConnection
	{
		private AbstractAdapter adapter = null;
		private bool             isFree = true;
		
		public AbstractAdapter Adapter
		{
			set {adapter = value;}
			get {return adapter;}
		}
		
		public bool IsFree()
		{
			return isFree;
		}
		
		public void Lock()
		{			
			isFree = false;
		}
		
		public void Release()
		{
			isFree = true;
		}
		
	}
	
	/*Пул соединений*/
	public class ConnectionPool : IDisposable
	{	
		private static Dictionary<string, ConnectionPool> pools = new Dictionary<string, ConnectionPool>();
		
		public static Dictionary<string, ConnectionPool> Pools
		{
			get {return pools;}
		}
		
		public static void PoolsPrepare(ILogger _logger)
		{
			Dictionary<string, Connection> connectionsConfig = Config.Connections;
			
			foreach (string name in connectionsConfig.Keys)
			{
				ConnectionPool pool = new ConnectionPool();
				pool.Logger = _logger;
				
				_logger.Info("Prepare pool:");
				foreach (string parameterName in connectionsConfig[name].Parameters.Keys)
				{
					string val = connectionsConfig[name].Parameters[parameterName];
					_logger.Info("		"+parameterName+"="+val);
					pool.Parameters.Add(parameterName, val);
					
				}											
				
				pools.Add(name, pool);
			}			
		}
		
		public static void PoolsInit(ILogger _logger)
		{			
			foreach (string name in pools.Keys)
			{
				_logger.Info("Init pool: "+name);
				pools[name].Init();				
			}
		}
		
		public static void PoolsDone(ILogger _logger)
		{
			foreach (string name in pools.Keys)
			{
				try
				{
					_logger.Info("Done pool"+name);
					pools[name].Done();										
				}
				catch (Exception _e)
				{
					try
					{
						_logger.Info("Exception in PoolsDone: "+_e.ToString());
					}
					catch (Exception _x)
					{								
					}
				}
			}
		}		
		
		
		public static readonly string     PoolSizeParam = "PoolSize";
		public static readonly string         NameParam = "Name";
		public static readonly string  AdapterTypeParam = "AdapterType";
		public static readonly string PoolUserNameParam = "PoolUserName";
		public static readonly string PoolPasswordParam = "PoolPassword";
		
		private Dictionary<string, string> parameters = 
											new Dictionary<string, string>();
		private Dictionary<Guid, PoolableConnection> connections = 
											new Dictionary<Guid, PoolableConnection>();
		
		private Queue<EventWaitHandle> waitQueue = new Queue<EventWaitHandle>();
		
		private Thread       waitQueueProcessor = null;
		private bool waitQueueProcessorStopFlag = false;
		private ILogger                  logger = SimpleLogger.DefaultLogger;
		
		private int                    poolSize = 0;
		private string                     name = null;
		private Type                adapterType = null;
		
		private string             poolUserName = null;
		private string              poolPassword = null;

		public string PoolPassword
		{
			get {return poolPassword;}
		}
		
		public string PoolUserName
		{
			get {return poolUserName;}
		}		
		
		public Type AdapterType
		{
			get {return adapterType;}
		}
		
		public string Name
		{
			get {return name;}
		}
		
		public int PoolSize
		{
			get {return poolSize;}
		}
		
		public ILogger Logger
		{
			set {logger = value;}
			get {return logger;}
		}
		
		public Dictionary<string, string> Parameters
		{
			get {return parameters;}			
		}
		
		public ConnectionPool()
		{			
		}		
		
		public void Init()
		{
				if (!Parameters.ContainsKey(PoolSizeParam))
				{
					throw new Exception(""+PoolSizeParam+" parameter not found");
				}
				else
				{
					poolSize = Convert.ToInt32(Parameters[PoolSizeParam]);
				}
				if (!Parameters.ContainsKey(NameParam))
				{
					throw new Exception(""+NameParam+" parameter not found");
				}
				else
				{
					name = Parameters[NameParam];
				}
				if (!Parameters.ContainsKey(NameParam))
				{
					throw new Exception(""+AdapterTypeParam+" parameter not found");
				}
				else
				{
					adapterType = Type.GetType(Parameters[AdapterTypeParam]);
				}				
				
				if (!Parameters.ContainsKey(PoolUserNameParam))
				{
					throw new Exception(""+PoolUserNameParam+" parameter not found");
				}
				else
				{
					poolUserName = Parameters[PoolUserNameParam];
				}				
				
				if (!Parameters.ContainsKey(PoolPasswordParam))
				{
					throw new Exception(""+PoolPasswordParam+" parameter not found");
				}
				else
				{
					poolPassword = Parameters[PoolPasswordParam];
				}	
				
				/*Поднимем соедиения для пула*/	
				for (int i=0; i<poolSize; i++)
				{
					AbstractAdapter adapter = (AbstractAdapter)Activator.CreateInstance(adapterType);
					adapter.Parameters = Parameters;
					PoolableConnection pc = new PoolableConnection();
					pc.Adapter = adapter;
										
					connections.Add(adapter.Guid, pc);
					
					adapter.Init();
				}				
			
				/*Запустим обработчик очереди ожидания*/
				waitQueueProcessor = new Thread(new ThreadStart(this.ProcessWaitQueue));
				waitQueueProcessor.Priority = ThreadPriority.AboveNormal;
				waitQueueProcessor.Name = "WaitQueueProcessor";
				waitQueueProcessor.Start();				
		}
		
		public void Done()
		{	
			/*Остановим обработчик очереди*/
			waitQueueProcessorStopFlag = true;
			
			/*Закроем соединения*/
			lock (connections)
			{
				foreach (Guid guid in connections.Keys)
				{
					try
					{
						connections[guid].Adapter.Done();
					}
					catch (Exception _e)
					{
						logger.Severe("Exception in ConnectionPool.Done: "+_e.ToString());
					}
				}
				connections.Clear();
			}
		}
		
		/*Обрабатываем очерель ожидания*/
		private void ProcessWaitQueue()
		{
			try
			{
				while (!waitQueueProcessorStopFlag)
				{
					lock (waitQueue)
					{
						if (waitQueue.Count > 0)
						{
							AbstractAdapter adapter = null;
							lock (connections)
							{
								adapter = GetFreeConnection(false);
								if (adapter != null)
								{
									waitQueue.Dequeue().Set();								
								}
							}
						}
					}
				}				
			}
			catch (Exception _e)
			{
				logger.Severe("Exception in ProcessWaitList: "+_e.ToString());
			}
			finally
			{
				/*Выведем из состояния ожидания всех тех кто там все еще находится*/
				lock (waitQueue)
				{
					while (waitQueue.Count > 0)
					{
						waitQueue.Dequeue().Set();
					}
				}
			}
				
		}
		
		private AbstractAdapter GetFreeConnection(bool _doLock)
		{			
			foreach (Guid guid in connections.Keys)
			{
				PoolableConnection pc = connections[guid];
				if (pc.IsFree())
				{
					if (_doLock)
					{
						pc.Lock();
					}
					return pc.Adapter;
				}
			}			
			return null;
		}
		
		public AbstractAdapter GetConnection(string _userName, string _password)
		{
			if ((!_userName.Equals(PoolUserName)) || (!_password.Equals(PoolPassword)))
			{
				throw new Exception("Wron user name or passowrd for access to connection pool");
			}
			
			AbstractAdapter adapter = null;
			lock (connections)
			{
				adapter = GetFreeConnection(true);
				if (adapter != null)
				{
					return adapter;
				}
			}			
			/*Текущий поток попадает в очередь ожидания*/
			EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
			lock (waitQueue)
			{
				waitQueue.Enqueue(waitHandle);															
			}
			/*И приостанавливается*/
			Mutex.WaitAny(new WaitHandle[] {waitHandle}, 30000);			
			/*До тех пор, пока его не пробудит обработчик ожидания для повторной попытки*/
			lock (connections)
			{
				adapter = GetFreeConnection(true);
				if (adapter != null)
				{
					return adapter;
				}
			}	
			/*Если же потоку так и не удалось получить соединение*/
			throw new Exception("Sorry, but current thread could not get free connection from pool. May be increase pool size?");
		}
		
		public void ReleaseConnection(AbstractAdapter _adapter)
		{
			lock (connections)
			{
				if (connections.ContainsKey(_adapter.Guid))
				{
					PoolableConnection pc = connections[_adapter.Guid];
					pc.Release();
				}
				else
				{
					throw new Exception("Unknown adapter for this pool: GUID="+_adapter.Guid.ToString());
				}
			}
		}
		
		public void Dispose()
		{			
			Done();						
		}
		
	}
}
