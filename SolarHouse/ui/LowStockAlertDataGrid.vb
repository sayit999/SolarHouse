Public Class LowStockAlertDataGrid
    Inherits DataGridView
    Public productsRunningLowTbl As DataTable = Nothing

    Public Sub loadGrid()

        SolarHouseDao.loadProductsRunningLow(productsRunningLowTbl)

        Me.DataSource = productsRunningLowTbl

        For i = 0 To Me.RowCount - 1
            If i Mod 2 = 0 Then
                Me.Rows(i).DefaultCellStyle.BackColor = Color.FromArgb(239, 254, 255)
            Else
                Me.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next i

    End Sub

End Class
