import argparse, socket

def recvall(sock, length):
    data = b''
    while len(data) < length:
        more = sock.recv(length - len(data))
        if not more:
            raise EOFError('was expecting %d bytes but only received'
                           ' %d bytes before the socket closed'
                           % (length, len(data)))
        data += more
    return data

def server(interface, port):
    record = ''
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    sock.bind((interface, port))
    sock.listen(200) #declare the socket is used to be a server, can't be repealed
                   # "1" here declare the max amount of connections 
    print('Listening at', sock.getsockname())
    while True:
        print('Waiting to accept a new connection')
        sc, sockname = sock.accept()  #create a new socket
        print('We have accepted a connection from', sockname)
        #print('  Socket name:', sc.getsockname()) #the name of server
        print('  Connector:', sc.getpeername())
        message = sc.recv(65535) #recvall(sc, 1024)
        recestr = message.decode()
        
        if recestr == "0":
            record = ""
            sc.close()
            continue
        
        if recestr[0] == "2":
            sc.sendall(record.encode(encoding="utf-8"))
            sc.close()
            continue
        
        record = record + message.decode()
        sc.sendall(record.encode(encoding="utf-8"))
        sc.close()    # this is necessary to complete the handshaking
        print('  Reply sent, socket closed')

if __name__ == '__main__':
    '''choices = {'client': client, 'server': server}
    parser = argparse.ArgumentParser(description='Send and receive over TCP')
    parser.add_argument('role', choices=choices, help='which role to play')
    parser.add_argument('host', help='interface the server listens at;'
                        ' host the client sends to')
    parser.add_argument('-p', metavar='PORT', type=int, default=1060,
                        help='TCP port (default 1060)')
    args = parser.parse_args()
    letter = input()
    function = choices[args.role]
    function('118.89.169.100', 1060, letter)'''
    server('10.154.192.161', 1060)