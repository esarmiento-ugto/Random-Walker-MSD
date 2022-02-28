Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Public Class Main
    Dim x As Single()
    Dim MSD As Single()
    Dim startpath As String
    Dim Nmax As Integer
    Dim maxMSD As Integer
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        startpath = Application.StartupPath
        Chart1.Series.Clear()
        Chart2.Series.Clear()
        Chart2.Series.Add("tray")
        Chart1.Series.Add("MSD")
        Chart2.Series("tray").ChartType = SeriesChartType.Point
        Chart1.Series("MSD").ChartType = SeriesChartType.Point
        Chart1.ChartAreas(0).AxisX.IsLogarithmic = False
        Chart1.ChartAreas(0).AxisY.IsLogarithmic = False
        Button1.Enabled = False
        Button1.Text = "In Progress"
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.RunWorkerAsync()
        TextBox1.Enabled = False
        TextBox2.Enabled = False
        TextBox3.Enabled = False
        TextBox4.Enabled = False
        TextBox5.Enabled = False
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim dx As Single = TextBox2.Text
        Dim temp As Single
        Nmax = TextBox1.Text
        maxMSD = TextBox3.Text
        ReDim x(Nmax - 1)
        ReDim MSD(maxMSD - 1)
        Dim cagesize As Single = TextBox5.Text
        x(0) = 0
        For i = 1 To Nmax - 1
            'x(i) = x(i - 1) + (2 * Rnd() - 1) * dx
            temp = 2 * Rnd() - 1
            x(i) = x(i - 1) + temp * dx
            If RadioButton2.Checked = True Then
                If x(i) > cagesize / 2 Then
                    x(i) = x(i - 1) - 2 * temp * dx
                End If
                If x(i) < -cagesize / 2 Then
                    x(i) = x(i - 1) - 2 * temp * dx
                End If
            End If
        Next
        For k = 1 To maxMSD - 1
            For m = 0 To Nmax - 1 - k
                MSD(k) = MSD(k) + (x(m + k) - x(m)) ^ 2
            Next
            MSD(k) = MSD(k) / (Nmax - 1 - k)
        Next
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Dim writer1 As New StreamWriter(startpath & "\tray.dat")
        Chart1.ChartAreas(0).AxisX.IsLogarithmic = True
        Chart1.ChartAreas(0).AxisY.IsLogarithmic = True
        For j = 0 To Nmax - 1
            writer1.WriteLine(x(j))
            Chart2.Series("tray").Points.AddXY(j * TextBox4.Text, x(j))
        Next
        writer1.Close()
        Dim writer2 As New StreamWriter(StartPath & "\MSD.dat")
        For l = 1 To maxMSD - 1
            writer2.WriteLine(l * TextBox4.Text & Chr(9) & MSD(l))
            Chart1.Series("MSD").Points.AddXY(l * TextBox4.Text, MSD(l))
        Next
        writer2.Close()
        Button1.Enabled = True
        Button1.Text = "Calculate"
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        TextBox3.Enabled = True
        TextBox4.Enabled = True
        TextBox5.Enabled = True
    End Sub
    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Enabled = True Then
            TextBox5.Enabled = False
        End If
    End Sub
    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Enabled = True Then
            TextBox5.Enabled = True
        End If
    End Sub
End Class
