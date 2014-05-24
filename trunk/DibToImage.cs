/* Saraff.Twain.dll �������� ��������� ��������, �������� ��� ��� �������, � ����� ����� ������ TWAIN ����������� �����������.
 * � SARAFF SOFTWARE (����������� ������), 2011.
 * ������ ���������� �������� ��������� ����������� ������������. 
 * �� ������ �������������� � �/��� �������������� � ������������ 
 * � ��������� ������ 3 ���� �� ������ ������ � ��������� ����� ������� 
 * ������ ����������� ������������ �������� ������������� ���������� GNU, 
 * �������������� Free Software Foundation.
 * �� �������������� ��� ���������� � ������� �� ��, ��� ��� ����� ��� 
 * ��������, ������ �� ������������� �� ��� ������� ��������, � ��� ����� 
 * �������� ��������� ��������� ��� ������� � ����������� ��� ������������� 
 * � ���������� �����. ��� ��������� ����� ��������� ���������� ������������ 
 * �� ����������� ������������ ��������� ������������� ���������� GNU.
 * ������ � ������ ����������� �� ������ ���� �������� ��������� ����������� 
 * ������������ �������� ������������� ���������� GNU. ���� �� ��� �� ��������, 
 * �������� �� ���� � Software Foundation, Inc., 59 Temple Place � Suite 330, 
 * Boston, MA 02111-1307, USA.
 * 
 * PLEASE SEND EMAIL TO:  twain@saraff.ru.
 */
using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Collections.Generic;


namespace Saraff.Twain {

    internal sealed class DibToImage {

        public static Image WithStream(IntPtr dibPtr) {
            MemoryStream _stream=new MemoryStream();
            BinaryWriter _writer=new BinaryWriter(_stream);

            BITMAPINFOHEADER _bmi=(BITMAPINFOHEADER)Marshal.PtrToStructure(dibPtr,typeof(BITMAPINFOHEADER));

            int _dibSize=_bmi.biSize+_bmi.biSizeImage+(_bmi.biClrUsed<<2);

            #region BITMAPFILEHEADER

            _writer.Write((ushort)0x4d42);
            _writer.Write(14+_dibSize);
            _writer.Write(0);
            _writer.Write(14+_bmi.biSize+(_bmi.biClrUsed<<2));

            #endregion

            #region BITMAPINFO and pixel data

            byte[] _data=new byte[_dibSize];
            Marshal.Copy(dibPtr,_data,0,_data.Length);
            _writer.Write(_data);

            #endregion

            return Image.FromStream(_stream);
        }

        [StructLayout(LayoutKind.Sequential,Pack=2)]
        private class BITMAPINFOHEADER {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }
    }
}
