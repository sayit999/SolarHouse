Imports MySql.Data.MySqlClient


Public Class SolarHouseDao
    Protected Shared Function connectToDB() As MySqlConnection

        Dim Password As String
        Dim Server_Name As String
        Dim User_ID As String
        Dim Database_Name As String
        'OMIT Dim rs statement


        Server_Name = "127.0.0.1"
        Database_Name = My.Settings.database_name
        User_ID = "root"
        Password = "arusha"
        Dim connection As MySqlConnection = New MySqlConnection()

        connection.ConnectionString = String.Format("server={0}; user id={1}; password={2}; database={3}; pooling=false", Server_Name, User_ID, Password, Database_Name)

        connection.Open()

        connectToDB = connection

    End Function

    Public Function prepStrSql(val As String) As String
        val = UIUtil.subsIfEmpty(val, "")
        prepStrSql = "'" + val.Replace("'", "''") + "'"
    End Function

    Public Shared Function isCodeBeingUsed(code As String, entityType As String)
        Dim Sql As String = ""
        If entityType = "expense_category" Then
            Sql = "select expense_category.expense_category_id "
            Sql += "from expense, expense_category "
            Sql += "where expense_category.expense_category_id = expense.expense_category_id "
            Sql += "and   expense_category_code = '" + code + "' "
        ElseIf entityType = "product" Then
            Sql = "select product.product_id "
            Sql += "from product, sale "
            Sql += "where product.product_id = sale.product_id "
            Sql += "and   product.product_code = '" + code + "' "
            Sql += "union "
            Sql += "select product.product_id "
            Sql += "from product, purchase "
            Sql += "where product.product_id = purchase.product_id "
            Sql += "and   product.product_code = '" + code + "' "
        ElseIf entityType = "supplier" Then
            Sql = "select supplier.supplier_id "
            Sql += "from supplier, purchase "
            Sql += "where supplier.supplier_id = purchase.supplier_id "
            Sql += "and   supplier.supplier_code = '" + code + " '"
            Sql += "union "
            Sql += "select supplier.supplier_id "
            Sql += "from supplier, payment "
            Sql += "where supplier.supplier_id = payment.supplier_id "
            Sql += "and   supplier.supplier_code = '" + code + "' "
        End If

        Dim ds As DataTable = New DataTable()
        Dim connection As MySqlConnection = connectToDB()
        Dim adp As MySqlDataAdapter = New MySqlDataAdapter(Sql, connection)
        adp.Fill(ds)
        isCodeBeingUsed = Not IsNothing(ds) AndAlso ds.Rows.Count > 0
    End Function

    Public Shared Sub close(con As MySqlConnection)
        If (Not IsNothing(con)) Then
            con.Close()
        End If
    End Sub

    Public Shared Sub close(adp As MySqlDataAdapter)
        If (Not IsNothing(adp)) Then
            adp.Dispose()
        End If
    End Sub

    Public Shared Sub execRetQry(sqlStr As String, ByRef dt As DataTable)
        Dim connection As MySqlConnection = Nothing
        Dim adp As MySqlDataAdapter = Nothing
        Try
            dt = New DataTable()
            connection = connectToDB()
            adp = New MySqlDataAdapter(sqlStr, connection)
            adp.Fill(dt)
        Finally
            close(adp)
            close(connection)
        End Try

    End Sub

    Protected Function executeSingleRetreivalSql(sql As String, cn As MySqlConnection) As Object
        Dim dt As DataTable = New DataTable()
        Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, cn)
        adp.Fill(dt)
        If (Not IsNothing(dt) AndAlso dt.Rows.Count > 0) Then
            executeSingleRetreivalSql = dt.Rows(0).Item(0)
        Else
            executeSingleRetreivalSql = Nothing
        End If
    End Function

    Protected Sub executeNonRetrievalSql(sqlStr As String, cn As MySqlConnection)
        Dim command As MySqlCommand = New MySqlCommand(sqlStr, cn)
        Dim cnt As Integer
        If LCase(sqlStr).IndexOf("insert") <> -1 OrElse LCase(sqlStr).IndexOf("update") <> -1 OrElse LCase(sqlStr).IndexOf("delete") <> -1 Then
            cnt = command.ExecuteNonQuery()
        End If
    End Sub

    Public Shared Sub loadProductCategories(ByRef ds As DataTable)
        Dim connection As MySqlConnection = connectToDB()
        Dim sql As String
        sql = "Select product_category_id as id, "
        sql += "      product_category_name as name "
        sql += "From product_category "
        sql += "Order By product_category_id "

        Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
        ds = New DataTable()
        adp.Fill(ds)
        connection.Close()

    End Sub

    Public Shared Sub loadQtyUOMs(ByRef ds As DataTable)
        Dim connection As MySqlConnection = connectToDB()
        Dim sql As String
        sql = "Select qty_uom_id as id, "
        sql += "      qty_uom_name as name "
        sql += "From qty_uom "
        sql += "Order By qty_uom_id "

        Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
        ds = New DataTable()
        adp.Fill(ds)
    End Sub

    Public Shared Sub loadProducts(ByRef ds As DataTable)
        Dim connection As MySqlConnection = connectToDB()
        Dim sql As String
        sql = "Select product.product_code, "
        sql += "      product.product_name, "
        sql += "      product_category.product_category_name, "
        sql += "      product.product_category_id, "
        sql += "      product.qty_available, "
        sql += "      qty_uom.qty_uom_name, "
        sql += "      product.qty_uom_id, "
        sql += "      product.acb_cost, "
        sql += "      product.retail_discount_room_percentage, "
        sql += "      product.min_ret_gross_profit_margin_percentage, "
        sql += "      product.retail_sale_price, "
        sql += "      low_stock_alert_qty, "
        sql += "      is_reorder, "
        sql += "      product.comments, "
        sql += "      product.product_id "
        sql += "From product, product_category, qty_uom "
        sql += "Where product.product_category_id = product_category.product_category_id "
        sql += "and   product.qty_uom_id = qty_uom.qty_uom_id "
        sql += "Order By product.product_code "

        Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
        ds = New DataTable()
        adp.Fill(ds)
    End Sub

    Public Shared Sub loadSuppliers(ByRef ds As DataTable)
        Dim connection As MySqlConnection = connectToDB()
        Dim sql As String
        sql = "select "
        sql += "      supplier_code,  "
        sql += "      supplier_name, "
        sql += "      comments, "
        sql += "      supplier_id "
        sql += "from supplier "
        sql += "order by supplier_code "
        Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
        ds = New DataTable()
        adp.Fill(ds)
    End Sub

    Public Shared Sub loadExpenseCategories(ByRef ds As DataTable)
        Dim connection As MySqlConnection = connectToDB()
        Dim sql As String

        sql = "select  "
        sql += "       expense_category_code"
        sql += "      ,expense_category_name "
        sql += "      ,expense_category_description "
        sql += "      ,expense_category_id "
        sql += "from expense_category "
        sql += "order by expense_category_code "

        Dim adp As MySqlDataAdapter = New MySqlDataAdapter(sql, connection)
        ds = New DataTable()
        adp.Fill(ds)
    End Sub



End Class
