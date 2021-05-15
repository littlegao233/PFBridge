
/**
 * .net : System
 */
declare namespace System {
    class Byte {
        constructor()
        constructor(size: number)
    }
    /**
   * Contains types that allow reading and writing to files and data streams, and types that provide basic file and directory support.
   * 包含允许读写文件和数据流的类型以及提供基本文件和目录支持的类型。
   */
    namespace IO {
        /**
         * Provides static methods for the creation, copying, deletion, moving, and opening of a single file, and aids in the creation of System.IO.FileStream objects.
         * 提供用于创建、复制、删除、移动和打开单一文件的静态方法，并协助创建 FileStream 对象。
         */
        module File {
            /**
             * Opens a text file, reads all the text in the file, and then closes the file.
             * 打开一个文本文件，读取文件中的所有文本，然后关闭此文件。
             * @param path The file to open for reading.
             * 要打开以进行读取的文件。
             */
            function ReadAllText(path: string): string
            /**
             * Opens a text file, reads all the text in the file, and then closes the file.
             * 打开一个文本文件，读取文件中的所有文本，然后关闭此文件。
             * @param path The file to open for reading.
             * 要打开以进行读取的文件。
             * @param encoding The encoding applied to the contents of the file.
             * 应用到文件内容的编码。
             */
            function ReadAllText(path: string, encoding: Text.Encoding): string
            /**
             * Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
             * 创建一个新文件，使用指定编码向其中写入指定的字符串，然后关闭文件。 如果目标文件已存在，则覆盖该文件。
             * @param path The file to write to.
             * 要打开以进行读取的文件。
             * @param contents The string to write to the file.
             * 要写入文件的字符串。
             */
            function WriteAllText(path: string, contents: string): string
            /**
             * Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
             * 创建一个新文件，使用指定编码向其中写入指定的字符串，然后关闭文件。 如果目标文件已存在，则覆盖该文件。
             * @param path The file to write to.
             * 要打开以进行读取的文件。
             * @param contents The string to write to the file.
             * 要写入文件的字符串。
             * @param encoding The encoding to apply to the string.
             * 应用于字符串的编码。
             */
            function WriteAllText(path: string, contents: string, encoding: Text.Encoding): string
            /**
             * 确定指定的文件是否存在。
             * @param path 要检查的文件。
             * @returns 如果调用方具有要求的权限并且 true 包含现有文件的名称，则为 path；否则为 false。 如果 false 为 path（一个无效路径或零长度字符串）,则此方法也将返回null。 如果调用方不具有读取指定文件所需的足够权限，则不引发异常并且该方法返回 false，这与 path 是否存在无关。
             */
            function Exists(path: string): boolean
        }
        module Path {
            /**
             * 将字符串数组组合成一个路径。
             * @param path1 路径1。
             * @param paths 由路径的各部分构成的数组。
             */
            function Combine(path1: string, ...paths: (string | string[])[]): string
            /**
             * 返回指定路径字符串的文件名和扩展名。
             * @param path 从中获取文件名和扩展名的路径字符串。
             */
            function GetFileName(path: string): string
        }
        module Directory {
            /**
             * 确定给定路径是否引用磁盘上的现有目录。
             * @param path 要测试的路径。
             * @returns 如果 path 指向现有目录，则为 true；如果该目录不存在或者在尝试确定指定目录是否存在时出错，则为 false。
             */
            function Exists(path: string): boolean
            /**
             * 在指定路径中创建所有目录和子目录，除非它们已经存在。
             * @param path 要创建的目录。
             * @returns 一个表示在指定路径的目录的对象。 无论指定路径的目录是否已经存在，都将返回此对象。
             */
            function CreateDirectory(path: string): DirectoryInfo
        }
        class DirectoryInfo extends FileSystemInfo {

        }
        class FileSystemInfo extends MarshalByRefObject {

        }
    }
    class MarshalByRefObject { }
    /**
   * Contains classes that represent ASCII and Unicode character encodings; abstract base classes for converting blocks of characters to and from blocks of bytes; and a helper class that manipulates and formats String objects without creating intermediate instances of String.
   * 包含表示 ASCII 和 Unicode 字符编码的类；用于字符块和字节块相互转换的抽象基类；以及无需创建 String 的中间实例即可操作 String 对象并设置其格式的帮助程序类。
   */
    namespace Text {
        abstract class Encoding {
            static ASCII: Encoding
            static Unicode: Encoding
            static UTF32: Encoding
            static UTF7: Encoding
            static UTF8: Encoding
            static Default: Encoding
            static BigEndianUnicode: Encoding
            GetBytes(s: string): Byte[] | number[]
            GetString(s: Byte[] | number[]): string
        }
        //  class ASCIIEncoding extends Encoding { }
        //  class UnicodeEncoding extends Encoding { }
        //  class UTF32Encoding extends Encoding { }
        //  class UTF7Encoding extends Encoding { }
        //  class UTF8Encoding extends Encoding { }
    }

    module Convert {
        function FromBase64String(s: string): Byte[] | number[]
        function ToBase64String(inArray: Byte[] | number[]): string
    }
    namespace Net.Sockets {
        //#region Enum
        const enum SocketType {
            /** 
             * 指定未知的 System.Net.Sockets.Socket 类型。
             */
            Unknown = -1,
            /** 
            * 支持可靠、双向、基于连接的字节流，而不重复数据，也不保留边界。 此类型的 System.Net.Sockets.Socket 与单个对方主机通信，并且在通信开始之前需要建立远程主机连接。
            * System.Net.Sockets.SocketType.Stream 使用传输控制协议 (ProtocolType.System.Net.Sockets.ProtocolType.Tcp)
            * 和 AddressFamily。System.Net.Sockets.AddressFamily.InterNetwork 地址族。
            */
            Stream = 1,
            /** 
            * 支持数据报，即最大长度固定（通常很小）的无连接、不可靠消息。 消息可能会丢失或重复并可能在到达时不按顺序排列。 System.Net.Sockets.Socket
            * 类型的 System.Net.Sockets.SocketType.Dgram 在发送和接收数据之前不需要任何连接，并且可以与多个对方主机进行通信。 System.Net.Sockets.SocketType.Dgram
            * 使用数据报协议 (ProtocolType.System.Net.Sockets.ProtocolType.Udp) 和 AddressFamily.System.Net.Sockets.AddressFamily.InterNetwork
            * 地址族。
             */
            Dgram = 2,
            /** 
            * 支持对基础传输协议的访问。 通过使用 System.Net.Sockets.SocketType.Raw，可以使用 Internet 控制消息协议 (ProtocolType.System.Net.Sockets.ProtocolType.Icmp)
            * 和 Internet 组管理协议 (ProtocolType.System.Net.Sockets.ProtocolType.Igmp) 这样的协议来进行通信。
            * 在发送时，您的应用程序必须提供完整的 IP 标头。 所接收的数据报在返回时会保持其 IP 标头和选项不变。
             */
            Raw = 3,
            /** 
            * 支持无连接、面向消息、以可靠方式发送的消息，并保留数据中的消息边界。 RDM（以可靠方式发送的消息）消息会依次到达，不会重复。 此外，如果消息丢失，将会通知发送方。
            * 如果使用 System.Net.Sockets.SocketType.Rdm 初始化 System.Net.Sockets.Socket，则在发送和接收数据之前无需建立远程主机连接。
            * 利用 System.Net.Sockets.SocketType.Rdm，您可以与多个对方主机进行通信。
            */
            Rdm = 4,
            /** 
            * 在网络上提供排序字节流的面向连接且可靠的双向传输。 System.Net.Sockets.SocketType.Seqpacket 不重复数据，它在数据流中保留边界。
            * System.Net.Sockets.SocketType.Seqpacket 类型的 System.Net.Sockets.Socket 与单个对方主机通信，并且在通信开始之前需要建立远程主机连接。
             */
            Seqpacket = 5
        }
        const enum ProtocolType {
            /** 
            * 未知的协议。
            */
            Unknown = -1,
            /** 
            * Internet 协议。
            */
            IP = 0,
            /** 
            * IPv6 逐跳选项标头。
            */
            IPv6HopByHopOptions = 0,
            /** 
            * 未指定的协议。
            */
            Unspecified = 0,
            /** 
            * Internet 控制消息协议。
            */
            Icmp = 1,
            /** 
            * Internet 组管理协议。
            */
            Igmp = 2,
            /** 
            * 网关到网关协议。
            */
            Ggp = 3,
            /** 
            * Internet 协议版本 4。
            */
            IPv4 = 4,
            /** 
            * 传输控制协议。
            */
            Tcp = 6,
            /** 
            * PARC 通用数据包协议。
            */
            Pup = 12,
            /** 
            * 用户数据报协议。
            */
            Udp = 17,
            /** 
            * Internet 数据报协议。
            */
            Idp = 22,
            /** 
            * Internet 协议版本 6 (IPv6)。
            */
            IPv6 = 41,
            /** 
            * IPv6 路由标头。
            */
            IPv6RoutingHeader = 43,
            /** 
            * IPv6 片段标头。
            */
            IPv6FragmentHeader = 44,
            /** 
            * IPv6 封装安全负载标头。
            */
            IPSecEncapsulatingSecurityPayload = 50,
            /** 
            * IPv6 身份验证标头。 有关详细信息，请参阅 https:*www.ietf.org 上的 RFC 2292，第 2.2.1 节。
            */
            IPSecAuthenticationHeader = 51,
            /** 
            * IPv6 的 Internet 控制消息协议。
            */
            IcmpV6 = 58,
            /** 
            * IPv6 无下一个标头。
            */
            IPv6NoNextHeader = 59,
            /** 
            * IPv6 目标选项标头。
            */
            IPv6DestinationOptions = 60,
            /** 
            * 网络磁盘协议（非正式）。
            */
            ND = 77,
            /** 
            * 原始 IP 数据包协议。
            */
            Raw = 255,
            /** 
            * Internet 数据包交换协议。
            */
            Ipx = 1000,
            /** 
            * 顺序包交换协议。
            */
            Spx = 1256,
            /** 
            * 顺序包交换版本 2 协议。
            */
            SpxII = 1257
        }
        const enum AddressFamily {
            /** 
            * 未知的地址族。
            */
            Unknown = -1,
            /** 
            * 未指定的地址族。
            */
            Unspecified = 0,
            /** 
            * Unix 本地到主机地址。
            */
            Unix = 1,
            /** 
            * IP 版本 4 的地址。
            */
            InterNetwork = 2,
            /** 
            * ARPANET IMP 地址。
            */
            ImpLink = 3,
            /** 
            * PUP 协议的地址。
            */
            Pup = 4,
            /** 
            * MIT CHAOS 协议的地址。
            */
            Chaos = 5,
            /** 
            * Xerox NS 协议的地址。
            */
            NS = 6,
            /** 
            * IPX 或 SPX 地址。
            */
            Ipx = 6,
            /** 
            * ISO 协议的地址。
            */
            Iso = 7,
            /** 
            * OSI 协议的地址。
            */
            Osi = 7,
            /** 
            * 欧洲计算机制造商协会 (ECMA) 地址。
            */
            Ecma = 8,
            /** 
            * Datakit 协议的地址。
            */
            DataKit = 9,
            /** 
            * CCITT 协议（如 X.25）的地址。
            */
            Ccitt = 10,
            /** 
            * IBM SNA 地址。
            */
            Sna = 11,
            /** 
            * DECnet 地址。
            */
            DecNet = 12,
            /** 
            * 直接数据链接接口地址。
            */
            DataLink = 13,
            /** 
            * LAT 地址。
            */
            Lat = 14,
            /** 
            * NSC Hyperchannel 地址。
            */
            HyperChannel = 15,
            /** 
            * AppleTalk 地址。
            */
            AppleTalk = 16,
            /** 
            * NetBios 地址。
            */
            NetBios = 17,
            /** 
            * VoiceView 地址。
            */
            VoiceView = 18,
            /** 
            * FireFox 地址。
            */
            FireFox = 19,
            /** 
            * Banyan 地址。
            */
            Banyan = 21,
            /** 
            * 本机 ATM 服务地址。
            */
            Atm = 22,
            /** 
            * IP 版本 6 的地址。
            */
            InterNetworkV6 = 23,
            /** 
            * Microsoft 群集产品的地址。
            */
            Cluster = 24,
            /** 
            * IEEE 1284.4 工作组地址。
            */
            Ieee12844 = 25,
            /** 
            * IrDA 地址。
            */
            Irda = 26,
            /** 
            * 支持网络设计器 OSI 网关的协议的地址。
            */
            NetworkDesigners = 28,
            /** 
            * MAX 地址。
            */
            Max = 29
        }
        /** 指定套接字发送和接收行为。 */
        const enum SocketFlags {
            /** 不对此调用使用任何标志。 */
            None = 0,
            /** 处理带外数据。 */
            OutOfBand = 1,
            /** 快速查看传入消息。 */
            Peek = 2,
            /** 不使用路由表进行发送。 */
            DontRoute = 4,
            /** 提供用于发送和接收数据的 WSABUF 结构数的标准值。 .NET Framework 4.5 上不使用或支持此值。 */
            MaxIOVectorLength = 16,
            /** 消息太大，无法放入指定的缓冲区，并且已被截断。 */
            Truncated = 256,
            /** 指示控制数据无法放入 64 KB 的内部缓冲区且已被截断。 */
            ControlDataTruncated = 512,
            /** 指示广播数据包。 */
            Broadcast = 1024,
            /** 指示多播数据包。 */
            Multicast = 2048,
            /** 部分发送或接收消息。 */
            Partial = 32768
        }
        //#endregion
        class Socket {

        }
    }
    class Type {
        static GetType(name: string): any
    }
    class Enum {
        static ToObject<T>(t: Type, value: number): T
        static Parse<T>(t: Type, name: string): T
    }
    namespace Collections {
        namespace ObjectModel {
            class ReadOnlyCollection<T>{
                [index: number]: T
                public get Count(): number
                Contains(item: T): boolean
                IndexOf(item: T): number
            }
        }
        namespace Generic {
            class List<T> extends Array<T>{
                [index: number]: T
                constructor()
                constructor(x: any)
                Contains(item: T): boolean
                public get Count(): number
                IndexOf(item: T, index: number): number
                IndexOf(item: T, index: number, count: number): number
            }
        }
    }
    namespace Threading {
        namespace Tasks {
            class Task {
                /**
                 * 创建一个在指定的时间间隔后完成的可取消任务。
                 * @param millisecondsDelay 在完成返回的任务前要等待的毫秒数；如果无限期等待，则为 -1。
                 */
                static Delay(millisecondsDelay: number): Task
                /**
                 * 将在线程池上运行的指定工作排队，并返回代表该工作的 System.Threading.Tasks.Task 对象。
                 * @param act 以异步方式执行的函数。
                 */
                static Run(action: (() => void)): Task
                /**
                 * 等待提供的所有 System.Threading.Tasks.Task 对象完成执行过程。
                 * @param tasks 要等待的 System.Threading.Tasks.Task 实例的数组。
                 */
                static WaitAll(...tasks: (Task | Task[])[]): void
                /**
                * 等待提供的任一 System.Threading.Tasks.Task 对象完成执行过程。
                * @param tasks 要等待的 System.Threading.Tasks.Task 实例的数组。
                */
                static WaitAny(...tasks: (Task | Task[])[]): void
                /**
                 * 创建一个在目标 System.Threading.Tasks.Task 完成时异步执行的延续任务。
                 * @param continuationAction 在 System.Threading.Tasks.Task 完成时要运行的操作。 在运行时，委托将作为一个自变量传递给完成的任务。
                 */
                ContinueWith(continuationAction: (() => void)): Task
                /**
                 * 等待 System.Threading.Tasks.Task 在指定的毫秒数内完成执行。
                 * @param millisecondsTimeout 等待的毫秒数，或为 System.Threading.Timeout.Infinite (-1)，表示无限期等待。
                 */
                Wait(millisecondsTimeout: number): boolean
                /** 
                 * 释放 System.Threading.Tasks.Task 类的当前实例所使用的所有资源。
                 */
                Dispose(): void
            }
        }
        class Thread {
            constructor(start: (() => void))
            constructor(start: (() => void), maxStackSize: number)
            constructor(start: ((obj: any) => void))
            constructor(start: ((obj: any) => void), maxStackSize: number)
            public get ApartmentState(): ApartmentState
            public set ApartmentState(v: ApartmentState)
            /**
             * 将当前线程挂起指定的毫秒数。
             * @param millisecondsTimeout 挂起线程的毫秒数。 如果 millisecondsTimeout 参数的值为零，则该线程会将其时间片的剩余部分让给任何已经准备好运行的、具有同等优先级的线程。如果没有其他已经准备好运行的、具有同等优先级的线程，则不会挂起当前线程的执行。
             */
            Sleep(millisecondsTimeout: number): void
            /**
             * 导致操作系统将当前实例的状态更改为 System.Threading.ThreadState.Running。
             */
            Start(): void
            public static get CurrentThread(): Thread

            SetApartmentState(state: ApartmentState): void
        }
        const enum ApartmentState {
            /** System.Threading.Thread 将创建并进入一个单线程单元。 */
            STA = 0,
            /** System.Threading.Thread 将创建并进入一个多线程单元。*/
            MTA = 1,
            /** 尚未设置 System.Threading.Thread.ApartmentState 属性。*/
            Unknown = 2
        }
    }
}

//declare const System: typeof SystemA;
