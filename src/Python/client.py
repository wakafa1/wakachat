import argparse, socket

def client(host, port, letter):
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((host, port))
    print('Client has been assigned socket name', sock.getsockname())
    sock.sendall(letter.encode(encoding="utf-8"))
    reply = sock.recv(1024)#recvall(sock, 1024)
    
    print('The server said', repr(reply))
    sock.close()

if __name__ == '__main__':
    '''choices = {'client': client, 'server': server}
    parser = argparse.ArgumentParser(description='Send and receive over TCP')
    parser.add_argument('role', choices=choices, help='which role to play')
    parser.add_argument('host', help='interface the server listens at;'
                        ' host the client sends to')
    parser.add_argument('-p', metavar='PORT', type=int, default=1060,
                        help='TCP port (default 1060)')
    args = parser.parse_args()'''
    letter = input()
    '''function = choices[args.role]
    function('118.89.169.100', 1060, letter)'''
    client('118.89.169.100', 1060, letter)