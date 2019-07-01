import socket

TCP_IP = '127.0.0.1'
TCP_PORT = 4401
BUFFER_SIZE = 32



sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.bind((TCP_IP, TCP_PORT))
sock.listen(1)

try:
	while True:
		conn, addr = sock.accept()
		print('Connection address: ' + str(addr))

		recievedMessage = ''
		while 1:
			data = conn.recv(BUFFER_SIZE)
			if not data:
				break
			recievedMessage = recievedMessage + data.decode()
		print('   Recieved data: \n' + recievedMessage)
		conn.close()
except:
	print('There was some error, closing conn')
	conn.close()

conn.close()