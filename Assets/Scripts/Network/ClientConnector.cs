using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using UnityEngine;

//
// Summary:
//  Provides client connections for TCP network services.
//  a network communication component.
//  This will be the basis for passing JSON in and out of the network connection. 
//  This component will be included in the Unity project as a shared object. 
public class ClientConnector
{
    public delegate void callbackDelegate(string message);

    byte[] readBuffer;
    TcpClient tcpClient = null;
    callbackDelegate theCallBack = null;

    //
    // Summary:
    //     init() should set up the port and address of a listening server to connect to.
    //
    // Parameters:
    //   serverIP:
    //     The DNS name of the remote host to which you intend to connect.
    //
    //   portNumber:
    //     The port number of the remote host to which you intend to connect.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     The hostname parameter is null.
    //
    //   System.ArgumentException:
    //     The portNumber argument is invalid or The serverIP argument is invalid.
    //
    //   System.ArgumentOutOfRangeException:
    //     The port parameter is not between System.Net.IPEndPoint.MinPort and System.Net.IPEndPoint.MaxPort.
    //
    //   System.Net.Sockets.SocketException:
    //     An error occurred when accessing the socket. See the Remarks section for
    //     more information.
    public int init(int portNumber, string serverIP)
    {
        if (portNumber < 0 || portNumber > 65535)
        {
            Debug.Log("init : The portNumber argument is invalid.");
            throw new ArgumentException("The portNumber argument is invalid.");
        }

        if (string.IsNullOrEmpty(serverIP))
        {
            Debug.Log("init : The serverIP argument is invalid.");
            throw new ArgumentException("The serverIP argument is invalid.");
        }

        try
        {
            tcpClient = new TcpClient(serverIP, portNumber);
        }
        catch (SocketException ex)
        {
            Debug.Log("init SocketException : " + ex.Message);
            throw ex;
        }
        catch (Exception ex)
        {
            Debug.Log("init Exception : " + ex.Message);
            throw ex;
        }
        return 0;
    }

    //
    // Summary:
    //     Gets or sets the size of the receive buffer.
    //
    // Returns:
    //     The size of the receive buffer, in bytes. The default value is 8192 bytes.
    //
    // Remarks
    //      The ReceiveBufferSize property gets or sets the number of bytes that you are expecting 
    //      to store in the receive buffer for each read operation. This property actually manipulates
    //      the network buffer space allocated for receiving incoming data.
    //      Your network buffer should be at least as large as your application buffer to ensure that
    //      the desired data will be available when you call the NetworkStream.Read method. 
    //      Use the ReceiveBufferSize property to set this size. If your application will be receiving bulk data,
    //      you should pass the Read method a very large application buffer.
    //      If the network buffer is smaller than the amount of data you request in the Read method,
    //      you will not be able to retrieve the desired amount of data in one read operation. 
    //      This incurs the overhead of additional calls to the Read method.
    //
    // Exceptions:
    //   System.Net.Sockets.SocketException:
    //     An error occurred when setting the buffer size.-or-In .NET Compact Framework
    //     applications, you cannot set this property. For a workaround, see the Platform
    //     Note in Remarks.
    public void SetReceiveBufferSize(int ReceiveBufferSize)
    {
        if (ReceiveBufferSize <= 0)
        {
            Debug.Log("init : The ReceiveBufferSize argument is invalid.");
            throw new ArgumentException("The ReceiveBufferSize argument is invalid.");
        }

        try
        {
            tcpClient.ReceiveBufferSize = ReceiveBufferSize;
        }
        catch (SocketException ex)
        {
            Debug.Log("SetReceiveBufferSize SocketException : " + ex.Message);
            throw ex;
        }
        catch (Exception ex)
        {
            Debug.Log("SetReceiveBufferSize Exception : " + ex.Message);
            throw ex;
        }
    }

    //
    // Summary:
    //     Disposes this ClientConnector instance and requests that the
    //     underlying TCP connection be closed.
    public void uninit()
    {
        if (tcpClient != null)
            tcpClient.Close();

        tcpClient = null;
    }

    //
    // Summary:
    //     setCallackPointer() will receive incoming data from the server connection.
    //
    // Parameters:
    //   theCallBack:
    //     An System.IAsyncResult object returned by a call to Overload:System.Net.Sockets.TcpClient.BeginConnect.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     The theCallBack parameter is null.
    //
    //   System.ArgumentOutOfRangeException:
    //     The offset parameter is less than 0.-or- The offset parameter is greater
    //     than the length of the buffer paramater.-or- The size is less than 0.-or-
    //     The size is greater than the length of buffer minus the value of the offset
    //     parameter.
    //
    //   System.IO.IOException:
    //     The underlying System.Net.Sockets.Socket is closed.-or- There was a failure
    //     while reading from the network. -or-An error occurred when accessing the
    //     socket. See the Remarks section for more information.
    //
    //   System.ObjectDisposedException:
    //     The System.Net.Sockets.NetworkStream is closed.
    //
    //   System.InvalidOperationException:
    //     The System.Net.Sockets.TcpClient is not connected to a remote host.
    //
    //   System.ObjectDisposedException:
    //     The System.Net.Sockets.TcpClient has been closed.
    public void setCallackPointer(callbackDelegate theCallBack)
    {
        if (theCallBack == null)
            throw new ArgumentNullException("The theCallBack argument is invalid.");

        try
        {
            Debug.Log("tcpClient.ReceiveBufferSize : " + tcpClient.ReceiveBufferSize);
            this.theCallBack = theCallBack;
            readBuffer = new byte[tcpClient.ReceiveBufferSize];
            tcpClient.GetStream().BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(doRead), tcpClient);
        }
        catch (SocketException ex)
        {
            Debug.Log("setCallackPointer SocketException : " + ex.Message);
            throw ex;
        }
        catch (Exception ex)
        {
            Debug.Log("setCallackPointer Exception : " + ex.Message);
            throw ex;
        }
    }
    
    //
    // Summary:
    //     it is executed when System.Net.Sockets.NetworkStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object) completes.
    //
    // Parameters:
    //   asyncResult:
    //    Represents the status of an asynchronous operation.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     The theCallBack parameter is null.
    //
    //   System.ArgumentOutOfRangeException:
    //     The offset parameter is less than 0.-or- The offset parameter is greater
    //     than the length of the buffer paramater.-or- The size is less than 0.-or-
    //     The size is greater than the length of buffer minus the value of the offset
    //     parameter.
    //
    //   System.IO.IOException:
    //     The underlying System.Net.Sockets.Socket is closed.-or- There was a failure
    //     while reading from the network. -or-An error occurred when accessing the
    //     socket. See the Remarks section for more information.
    //     The underlying System.Net.Sockets.Socket is closed.-or- An error occurred
    //     when accessing the socket. See the Remarks section for more information.
    //
    //   System.ObjectDisposedException:
    //     The System.Net.Sockets.NetworkStream is closed.
    //     The System.Net.Sockets.TcpClient has been closed.
    //
    //   System.InvalidOperationException:
    //     The System.Net.Sockets.TcpClient is not connected to a remote host.
    //
    //   System.ArgumentOutOfRangeException:
    //     index or count is less than zero.-or- index and count do not denote a valid
    //     range in bytes.
    //
    //   System.Text.DecoderFallbackException:
    //     A fallback occurred (see Understanding Encodings for complete explanation)-and-System.Text.Encoding.DecoderFallback
    //     is set to System.Text.DecoderExceptionFallback.
    private void doRead(IAsyncResult asyncResult)
    {
        int bytesRead = 0;
        string message = string.Empty;

        try
        {
            // Finish asynchronous read into readBuffer and return number of bytes read.
            bytesRead = tcpClient.GetStream().EndRead(asyncResult);

            if (bytesRead < 1)
            {
                Debug.Log("Client Disconnected");
                return;
            }

            // Convert the byte array the message was saved into.
            message = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);

            // Start a new asynchronous read into readBuffer.
            tcpClient.GetStream().BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(doRead), null);

            if (theCallBack != null)
                theCallBack(message);
        }
        catch (SocketException ex)
        {
            Debug.Log("doRead SocketException : " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.Log("doRead Exception : " + ex.Message);
        }
    }

    //
    // Summary:
    //     it is executed when System.Net.Sockets.NetworkStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object) completes.
    //
    // Parameters:
    //   message:
    //    The string to write to the stream. If value is null, nothing is written.
    //
    // Exceptions:
    //   System.ArgumentException:
    //     stream is not writable.
    //
    //   System.ArgumentNullException:
    //     stream is null.
    //
    //   System.ObjectDisposedException:
    //     System.IO.StreamWriter.AutoFlush is true or the System.IO.StreamWriter buffer
    //     is full, and current writer is closed.
    //     The current writer is closed.
    //
    //   System.NotSupportedException:
    //     System.IO.StreamWriter.AutoFlush is true or the System.IO.StreamWriter buffer
    //     is full, and the contents of the buffer cannot be written to the underlying
    //     fixed size stream because the System.IO.StreamWriter is at the end the stream.
    //
    //   System.IO.IOException:
    //     An I/O error occurs.
    //
    //   System.Text.EncoderFallbackException:
    //     The current encoding does not support displaying half of a Unicode surrogate
    //     pair.
    public void sendRawMessage(string message)
    {
        try
        {
            StreamWriter writer = new StreamWriter(tcpClient.GetStream());
            writer.Write(message);
            writer.Flush();
        }
        catch(SocketException ex)
        {
            Debug.Log("sendRawMessage SocketException : " + ex.Message);
            throw ex;
        }
        catch (Exception ex)
        {
            Debug.Log("sendRawMessage Exception : " + ex.Message);
            throw ex;
        }
    }

}