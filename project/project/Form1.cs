using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;


namespace project
{
    public partial class Form1 : Form
    {
        Hashtable processes_table;
        int number_of_processes;
        double[] bursttime;
        double[] arrivaltime;

        public Form1()
        {
            InitializeComponent();
            processes_table = new Hashtable();


        }



        private void button2_Click(object sender, EventArgs e)
        {
            get_data();
            fcfs();
            sjf();
            roundrobin();
            multilevel();
        }

        private void get_data()
        {
            number_of_processes = int.Parse(textBox5.Text);
            bursttime = new double[number_of_processes];
            arrivaltime = new double[number_of_processes];

            if (textBox6.Text.Trim() != null)
            {

                string[] burst = textBox6.Text.Split(' ');
                string[] arrival = textBox7.Text.Split(' ');

                for (int x = 0; x < number_of_processes; x++)
                {
                    bursttime[x] = double.Parse(burst[x]);

                    if (arrival[0].ToString().Trim() != "")
                        arrivaltime[x] = double.Parse(arrival[x]);
                }
            }

        }

        private void fcfs()
        {
            textBox3.Text = null;
            textBox1.Text = null;
            double[] finished = new double[number_of_processes];
            double sum = 0;
            for (int x = 1, y = 0; x <= number_of_processes; x++, y++)
            {
                textBox3.Text += "p" + x.ToString() + "***";
                sum += bursttime[y];
                textBox1.Text += sum.ToString() + "***";
            }

        }

        private void sjf()
        {
            
            double[] arrival = arrivaltime; // arrivaltime && bursttime from the getdata() method
            double[] burst_time = new double[bursttime.Length];
            int[] finish_time = new int[number_of_processes];
            
            double total_time = 0;
            Queue<int> total_processes = new Queue<int>();
            Queue<double> sequence = new Queue<double>() ;
            
            for(int x=0;x<bursttime.Length;x++)
            {
                burst_time[x] = bursttime[x];
            }


            foreach(double value in burst_time) // Get the total time in  the interval ;
            {
                total_time += value;
            }

            for(int count=0;count<total_time;count++)
            {

                for(int y=0;y<arrival.Length;y++)
                {
                    if (arrival[y] == count)
                        total_processes.Enqueue(y);
                }

                double minimum = 9999;
                int minimum_index = 0;
                for(int y=0;y<total_processes.Count;y++) // to get the process that had the minimum burst time
                {
                    int index = total_processes.ElementAt(y);
                    if (burst_time[index] < minimum && burst_time[index] != 0)
                    {
                        minimum = burst_time[index];
                        minimum_index = index;
                    }
                }

                if (minimum != 9999) // Change the values 
                {
                    sequence.Enqueue(minimum_index);
                    finish_time[minimum_index]++;
                    burst_time[minimum_index]--;
                }
            }

            foreach(double val in sequence) // to display the sequence in the textbox
            {
                textBox8.Text += (int.Parse(val.ToString())).ToString() + "***";
            }

            for(int x=0;x<total_time;x++) // to display the time of the sequence
            {
                textBox9.Text += x.ToString() + "***";
            }

        }


        private void roundrobin()
        {
            try
            {

                int quantum = int.Parse(textBox10.Text);
                double full_time = 0.0;
                Queue<int> p = new Queue<int>();
                double[] burst_time = bursttime;

                textBox11.Text = "";

                foreach (double value in bursttime)
                {
                    full_time += value;
                }




                if (!checkBox1.Checked)
                {
                    for (int counter = 0; counter < full_time; counter++)
                    {

                        for (int process = 0; process < arrivaltime.Length; process++)
                        {
                            if (counter == arrivaltime[process])
                                p.Enqueue(process);

                        }

                        if (counter % quantum == 0)
                        {
                            p.Enqueue(p.Dequeue());

                        }

                        if (burst_time[p.First()] == 0)
                            p.Dequeue();


                        burst_time[p.First()]--;
                        textBox11.Text += "p" + (p.First() + 1) + "***";

                    }
                }
                else
                {
                    string[] sequence = textBox12.Text.Split(' ');

                    for (int x = 0; x < sequence.Length; x++)
                    {
                        for (int y = 0; y < sequence.Length; y++)
                        {
                            if (sequence[y] == x.ToString())
                                p.Enqueue(int.Parse(sequence[y]));
                        }
                    }

                    for (int counter = 0; counter < full_time; counter++)
                    {
                        burst_time[p.First()]--;
                        textBox11.Text += "p" + (p.First() + 1) + "***";


                        if (counter % quantum == 0)
                        {
                            p.Enqueue(p.Dequeue());

                        }

                        if (burst_time[p.First()] == 0)
                            p.Dequeue();
                    }

                }
            }catch
            {

            }
        }



        private void multilevel()
        {
            Queue<int> firstqueue = new Queue<int>();
            Queue<int> secondqueue = new Queue<int>();
            Queue<int> lastqueue = new Queue<int>();
            int first_quantum = int.Parse(textBox4.Text);
            int second_quantum = int.Parse(textBox2.Text);

            double[] burst_time = new double[bursttime.Length];

            for (int x = 0; x < burst_time.Length; x++)
            {
                burst_time[x] = bursttime[x];
            }

            for (int count = 0; count < number_of_processes; count++)
            {
                firstqueue.Enqueue(count);
            }

            while (firstqueue.Count != 0)
            {
                int value = firstqueue.Dequeue();

                if (burst_time[value] <= first_quantum)
                    textBox13.Text += "p" + (value +1) + "***";
                else
                {
                    burst_time[value] -= first_quantum;
                    secondqueue.Enqueue(value);
                }
            }

            while(secondqueue.Count != 0)
            {
                int value = secondqueue.Dequeue();

                if (burst_time[value] <= second_quantum)
                    textBox13.Text += "p" + (value +1) + "***";
                else
                {
                    burst_time[value] -= second_quantum;
                    lastqueue.Enqueue(value);
                }

            }

            while(lastqueue.Count != 0)
            {
                int value = lastqueue.Dequeue();

                textBox13.Text += "p" + (value +1) + "***";
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
