using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;

namespace Remote
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        // 생성자 선언
        SerialPort serialPort = new SerialPort();

        // 변수 선언
        bool[] buttonFlag = new bool[20];

        public MainWindow()
        {
            InitializeComponent();

            // buttonFlag 초기화
            for (int i = 0; i < 10; i++)
                buttonFlag[i] = false;
        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.Key + "");

        }

        // Window가 닫힐 때
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Threading.Thread.Sleep(100);

            if (serialPort.IsOpen == true)
                ComportClose();
        }

        // ComportComboBox에 연결된 Comport 확인 후 목록 생성
        private void ComportComboBox_DropDownOpened(object sender, EventArgs e)
        {
            if (ComportComboBox.IsFocused == true || BaudrateComboBox.IsFocused == true)
                ComportComboBox.Items.Clear();

            ComportComboBox.BeginInit();
            foreach (string getComport in SerialPort.GetPortNames())
                ComportComboBox.Items.Add(getComport);
            ComportComboBox.EndInit();
        }

        // BaudrateComboBox에 통신속도 목록 생성
        private void BaudrateComboBox_DropDownOpened(object sender, EventArgs e)
        {
            if (BaudrateComboBox.IsFocused == true || ComportComboBox.IsFocused == true)
                BaudrateComboBox.Items.Clear();

            BaudrateComboBox.Items.Add("9600");
            BaudrateComboBox.Items.Add("19200");
            BaudrateComboBox.Items.Add("115200");
            BaudrateComboBox.Items.Add("1000000");
            BaudrateComboBox.SelectedIndex = 3;
        }

        // Comport Open
        private bool ComportOpen()
        {
            try
            {
                string comportValue = ComportComboBox.Text;
                int baudrateValue = Convert.ToInt32(BaudrateComboBox.Text);

                serialPort = new SerialPort(comportValue, baudrateValue);
                serialPort.Encoding = Encoding.Default;
                serialPort.Parity = Parity.None;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;

                serialPort.Open();

                MessageBox.Show(serialPort.PortName + " 연결 성공");

                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);

                return false;
            }
        }

        // Comport Close
        private bool ComportClose()
        {
            try
            {
                serialPort.Close();
                serialPort.Dispose();
                MessageBox.Show(serialPort.PortName + " 연결 해제");

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                return false;
            }
        }

        // Serial 데이터 리시브
        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string readBuffer = serialPort.ReadLine();
                ControlDataSet();
            }
            catch
            {

            }
            System.Threading.Thread.Sleep(50);
        }

        // 제어 신호 전송
        private void ControlDataSet()
        {
            if(serialPort.IsOpen == true)
            {
                if (buttonFlag[1] == true)
                    serialPort.Write("q");
            }
        }
        // Open 버튼 클릭
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if(serialPort.IsOpen == true)
                MessageBox.Show(serialPort.PortName + "가 연결되어 있습니다.");
            else
                ComportOpen();
        }

        // Close 버튼 클릭
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.Sleep(100);

            if (serialPort.IsOpen == true)
                ComportClose();
            else
                MessageBox.Show(serialPort.PortName + "가 연결되지 않았습니다.");
        }

        private void LeftUpButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            buttonFlag[1] = true;
        }

        private void LeftUpButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            buttonFlag[1] = false;
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            buttonFlag[1] = true;
        }

        private void CenterButton_Click(object sender, RoutedEventArgs e)
        {+.3
            for (int i = 0; i < 20; i++)
            {
                buttonFlag[i] = false;
            }
        }
    }
}
