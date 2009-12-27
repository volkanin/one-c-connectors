import time
import stomp

from stomp import ConnectionListener

class TestListener(ConnectionListener):
    def __init__(self):
        self.errors = 0
        self.connections = 0
        self.messages = 0

    def on_error(self, headers, message):
        print('received an error %s' % message)
        self.errors = self.errors + 1

    def on_connecting(self, host_and_port):
        print('connecting %s %s' % host_and_port)
        self.connections = self.connections + 1

    def on_message(self, headers, message):
        print('received a message %s' % message)
        self.messages = self.messages + 1


s='<order-request>\n'+\
	'<shipment-date>11.11.2009</shipment-date>\n'+\
	'<order-id>1</order-id>\n'+\
	'<price-overhead>0.15</price-overhead>\n'+\
	'	<positions>\n'+\
	'		<position code="1" count="10" price="50"/>\n'+\
	'		<position code="2" count="5" price="30"/>\n'+\
	'	</positions>\n'+\
	'</order-request>'



print "Send:"
print s

connection = stomp.Connection([('127.0.0.1', 61613)], user='test', passcode='test')
connection.set_listener('', TestListener())
connection.start()
connection.connect(wait=True)
connection.send(s, destination='/queue/contr1In')
time.sleep(5)
connection.disconnect()

print "ok"

