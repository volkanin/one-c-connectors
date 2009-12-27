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


print "Recv:"

connection = stomp.Connection([('127.0.0.1', 61613)], user='test', passcode='test')
connection.set_listener('', TestListener())
connection.start()
connection.connect(wait=True)
connection.subscribe(destination='/queue/contr1Out', ack='auto')
time.sleep(5)
connection.disconnect()

print "ok"

