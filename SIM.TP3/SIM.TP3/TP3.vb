﻿Public Class TP3
    Dim maximo, minimo As Double

    Dim acumChi As Double

    Enum distribucion
        uniforme
        exponencial
        normal
        poisson
    End Enum

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmb_intervalos.Items.Add("5")
        cmb_intervalos.Items.Add("10")
        cmb_intervalos.Items.Add("20")
        cmb_intervalos.SelectedIndex = 0
        minimo = Nothing
        maximo = Nothing
    End Sub

    Private Function distUniforme(A As Double, B As Double)
        Dim res As Double

        res = A + Rnd() * (B - A)
        Return res
    End Function

    'Exponencial y Poisson no tienen que generar negativos.

    Private Function distExponencial(lambda As Double)
        Dim res As Double

        res = (-1 / lambda) * Math.Log(1 - Rnd())
        Return res
    End Function

    Private Function distNormal(media As Double, desviacion As Double)
        Dim res(1) As Double
        Dim rnd1 As Double = Rnd()
        Dim rnd2 As Double = Rnd()

        res(0) = (Math.Sqrt(-2 * Math.Log(rnd1)) * Math.Cos(2 * Math.PI * rnd2)) * desviacion + media
        res(1) = (Math.Sqrt(-2 * Math.Log(rnd1)) * Math.Sin(2 * Math.PI * rnd2)) * desviacion + media
        Return res
    End Function

    Private Function distPoisson(lambda As Double)
        Dim P As Double = 1
        Dim X As Double = -1
        Dim A As Double = Math.Exp(-lambda)

        Do
            P = P * Rnd()
            X += 1
        Loop While (P >= A)

        Return X
    End Function

    Private Sub agregarFilaGrid(index As Integer, rnd As Double)

        If index = 0 Then
            maximo = Math.Ceiling(rnd)
            minimo = Math.Floor(rnd)
        End If

        If rnd > maximo Then
            maximo = Math.Ceiling(rnd)

        End If

        If rnd < minimo Then
            minimo = Math.Floor(rnd)
        End If

        Me.Grid1.Rows.Add()
        Me.Grid1.Rows(index).Cells(0).Value = index + 1
        Me.Grid1.Rows(index).Cells(1).Value = rnd

    End Sub

    Private Sub generarRND1()

        For index = 0 To Integer.Parse(Me.txt_muestra1.Text) - 1

            Me.agregarFilaGrid(index, Me.distUniforme(Me.txt_A.Text, Me.txt_B.Text))

        Next

    End Sub

    Private Sub generarRND2()

        If Me.txt_lambda2.Text = "" Then    'Ver de hacerlo bien 
            Me.txt_lambda2.Text = 1 / (Me.txt_media2.Text)
        End If

        For index = 0 To Integer.Parse(txt_muestra2.Text) - 1
            Me.agregarFilaGrid(index, Me.distExponencial(Me.txt_lambda2.Text))
        Next

    End Sub

    Private Sub generarRND3()

        Dim v(Me.txt_muestra3.Text - 1) As Double
        Dim vaux(1) As Double
        Dim contador As Integer = 0
        Dim cantidad As Integer = Math.Ceiling(Me.txt_muestra3.Text / 2) 'parse

        For index = 0 To cantidad - 1
            vaux = Me.distNormal(Me.txt_media3.Text, Me.txt_desviacion3.Text)
            Me.agregarFilaGrid(contador, vaux(0))
            contador += 1

            If contador = Me.txt_muestra3.Text Then
                Exit For
            End If

            Me.agregarFilaGrid(contador, vaux(1))
            contador += 1
        Next

    End Sub

    Private Sub generarRND4()


        For index = 0 To Integer.Parse(txt_muestra4.Text) - 1
            Me.agregarFilaGrid(index, Me.distPoisson(Me.txt_lambda4.Text))
        Next

    End Sub

    Private Sub cargarGrid2(tamMuestra As Integer, dist As distribucion)

        Dim tamIntervalo As Double = (maximo - minimo) / Me.cmb_intervalos.SelectedItem
        Dim v(Me.cmb_intervalos.SelectedItem - 1) As Integer
        Dim fe As Double

        For index = 0 To tamMuestra - 1

            v(Math.Truncate((Me.Grid1.Rows(index).Cells(1).Value - minimo) / tamIntervalo)) += 1

        Next

        For index = 0 To Me.cmb_intervalos.SelectedItem - 1

            Select Case dist
                Case distribucion.uniforme
                    fe = tamMuestra / Me.cmb_intervalos.SelectedItem
                Case distribucion.exponencial
                    fe = ((1 - Math.Exp(-Me.txt_lambda2.Text * (minimo + tamIntervalo * (index + 1)))) - (1 - Math.Exp(-Me.txt_lambda2.Text * (minimo + tamIntervalo * index)))) * Me.txt_muestra2.Text 'Probabilidad acumulada del intervalo superior menos la del inferior

                Case distribucion.normal
                    fe = 0
                Case distribucion.poisson
                    fe = 0
            End Select

            acumChi += ((v(index) - fe) ^ 2) / fe 'Para comparar con chi tabulado

            Me.grid2.Rows.Add()
            Me.grid2.Rows(index).Cells(0).Value = "[" & (minimo + tamIntervalo * index) & " ; " & minimo + tamIntervalo * (index + 1) & "]"
            Me.grid2.Rows(index).Cells(1).Value = v(index)
            Me.grid2.Rows(index).Cells(2).Value = fe
            Me.grid2.Rows(index).Cells(3).Value = ((v(index) - fe) ^ 2) / fe
        Next

    End Sub

    Private Sub compararChi(glibertad As Integer)
        Dim v(28) As Double

        v(0) = 3.8415
        v(1) = 5.9915
        v(2) = 7.8147
        v(3) = 9.4877
        v(4) = 11.0705
        v(5) = 12.5916
        v(6) = 14.0671
        v(7) = 15.5073
        v(8) = 16.919
        v(9) = 18.307
        v(10) = 19.6752
        v(11) = 21.0261
        v(12) = 22.362
        v(13) = 23.6848
        v(14) = 24.9958
        v(15) = 26.2962
        v(16) = 27.5871
        v(17) = 28.8693
        v(18) = 30.1435
        v(19) = 31.4104
        v(20) = 32.6706
        v(21) = 33.9245
        v(22) = 35.1725
        v(23) = 36.415
        v(24) = 37.6525
        v(25) = 38.8851
        v(26) = 40.1133
        v(27) = 41.3372
        v(28) = 42.5569

        Me.txt_chiTab.Text = v(glibertad)
        Me.txt_chiCal.Text = acumChi
        If v(glibertad) > acumChi Then

            Me.lbl_resp.Text = "Se acepta la hipótesis"
        Else
            Me.lbl_resp.Text = "Se rechaza la hipótesis"
        End If

    End Sub

    Private Sub cargarGrid3B() 'Para reagrupar y mandar a la grilla
        'Me.grid3B.Rows.Clear()

        'Dim contadorSaltos = Me.obtenerIntervalos(Me.numerosChiB, Me.contadorChiB)
        'Dim auxiliar = Integer.Parse(Me.cmb_intervalosB.SelectedItem) / contadorSaltos 'cantidad de intervalos
        'Dim contSaltos As Integer = 0
        'Dim contChi As Double = 0
        'Dim extra As Integer

        'For index = 0 To auxiliar - 1

        '    Me.grid3B.Rows.Add()
        '    Me.grid3B.Rows(index).Cells(0).Value = index + 1

        '    If index = 0 Then 'solo se hace una vez en la primera vuelta para evitar problemas
        '        extra = Me.cmb_intervalosB.SelectedItem Mod contadorSaltos 'extra son los elementos que sobrarian luego de reagrupar
        '    End If

        '    For index2 = 1 To contadorSaltos + extra 'los elementos que sobran, los ponemos en el primer grupo y de ahi nos queda siempre una cantidad justa para agrupar

        '        Me.grid3B.Rows(index).Cells(1).Value += Me.grid1B.Rows(contSaltos).Cells(1).Value
        '        Me.grid3B.Rows(index).Cells(2).Value += Me.grid1B.Rows(contSaltos).Cells(2).Value
        '        contSaltos += 1
        '    Next

        '    extra = 0 'extra a partir de ahora no sirve entonces vale 0

        '    Me.grid3B.Rows(index).Cells(3).Value = ((Me.grid3B.Rows(index).Cells(1).Value - Me.grid3B.Rows(index).Cells(2).Value) ^ 2) / Me.grid3B.Rows(index).Cells(2).Value
        '    contChi += Me.grid3B.Rows(index).Cells(3).Value

        'Next

        'Me.txt_chiCalB.Text = contChi
        'Me.compararChi(NroPunto.b, Me.grid3B.Rows.Count)

    End Sub

    Private Sub cargarGraficoB()
        'El estilo del gráfico se edita en Propiedades, Series en la pestaña de diseño del form
        Me.Chart1.Series("fe").Points.Clear()
        Me.Chart1.Series("fo").Points.Clear()
        'Dim anchoInt = 1 / cantidadFilasGrid3
        Dim aux As Double = 0
        For i = 0 To Me.grid2.Rows.Count - 2
            'aux += anchoInt
            'Me.Chart1.Series("fe").Points.AddXY(aux, Me.grid2.Rows(i).Cells(2).Value)
            'Me.Chart1.Series("fo").Points.AddXY(aux, Me.grid2.Rows(i).Cells(1).Value)
            Me.Chart1.Series("fe").Points.AddXY(Me.grid2.Rows(i).Cells(0).Value, Me.grid2.Rows(i).Cells(2).Value)
            Me.Chart1.Series("fo").Points.AddXY(Me.grid2.Rows(i).Cells(0).Value, Me.grid2.Rows(i).Cells(1).Value)
        Next

    End Sub

    Private Sub cmd_generar1_Click(sender As Object, e As EventArgs) Handles cmd_generar1.Click
        Me.Grid1.Rows.Clear()
        Me.grid2.Rows.Clear()
        Me.txt_chiCal.Clear()
        Me.txt_chiTab.Clear()
        acumChi = 0
        Me.generarRND1()
        Me.cargarGrid2(Me.txt_muestra1.Text, distribucion.uniforme)
        Me.compararChi(Me.cmb_intervalos.SelectedItem() - 1)
        Me.cargarGraficoB()
        'Me.graficar(1)
    End Sub

    Private Sub cmd_generar2_Click(sender As Object, e As EventArgs) Handles cmd_generar2.Click
        Me.Grid1.Rows.Clear()
        Me.grid2.Rows.Clear()
        Me.txt_chiCal.Clear()
        Me.txt_chiTab.Clear()
        acumChi = 0
        Me.generarRND2()
        Me.cargarGrid2(Me.txt_muestra2.Text, distribucion.exponencial)
        Me.compararChi(Me.cmb_intervalos.SelectedItem() - 2)
        Me.cargarGraficoB()
    End Sub

    Private Sub cmd_generar3_Click(sender As Object, e As EventArgs) Handles cmd_generar3.Click
        Me.Grid1.Rows.Clear()
        Me.grid2.Rows.Clear()
        Me.generarRND3()
        'Me.cargarGrid2(Me.txt_muestra3.Text, distribucion.normal)
    End Sub

    Private Sub cmd_generar4_Click(sender As Object, e As EventArgs) Handles cmd_generar4.Click
        Me.Grid1.Rows.Clear()
        Me.grid2.Rows.Clear()
        Me.generarRND4()
        'Me.cargarGrid2(Me.txt_muestra4.Text, distribucion.poisson)

    End Sub
End Class