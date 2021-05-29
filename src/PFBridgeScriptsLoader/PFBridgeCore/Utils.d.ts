/// <reference path="./System.d.ts" />
 
declare interface UtilsNetSockets {
    Socket: Socket
}
declare interface Socket {
    CreateSocket(addressFamily: System.Net.Sockets.AddressFamily, socketType: System.Net.Sockets.SocketType, protocolType: System.Net.Sockets.ProtocolType): System.Net.Sockets.Socket
    CreateSocket(socketType: System.Net.Sockets.SocketType, protocolType: System.Net.Sockets.ProtocolType): System.Net.Sockets.Socket
    /**
     * 通过Socket对象发送指定数据
     * @param client Socket对象
     * @param data 数据
     * @param address 地址
     * @param port 端口
     */
    SendData(client: System.Net.Sockets.Socket, data: System.Byte[] | number[], address: string, port: number): void
    /**
     * 通过Socket对象接收指定数据
     * @param client Socket对象
     * @param size 要接收的字节数。
     */
    ReceiveData(client: System.Net.Sockets.Socket, size: number): System.Byte[] | number[]
    /**
     * 通过Socket对象接收指定数据
     * @param client Socket对象
     * @param size 要接收的字节数。
     * @param flag System.Net.Sockets.SocketFlags 值的按位组合。
     */
    ReceiveData(client: System.Net.Sockets.Socket, size: number,flag:System.Net.Sockets.SocketFlags): System.Byte[] | number[]
    /**
     * 通过Socket对象接收指定数据
     * @param client Socket对象
     * @param offset buffer 中存储所接收数据的位置
     * @param size 要接收的字节数。
     * @param flag System.Net.Sockets.SocketFlags 值的按位组合。
     */
    ReceiveData(client: System.Net.Sockets.Socket, offset:number, size: number,flag:System.Net.Sockets.SocketFlags): System.Byte[] | number[]
}
