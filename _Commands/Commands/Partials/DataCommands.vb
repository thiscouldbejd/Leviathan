Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Namespace Commands

	Partial Public Class DataCommands
		Implements IDisposable

		#Region " Public Constants "

			''' <summary>
			''' SQL Constant for an AND Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_AND As String = "AND"

			''' <summary>
			''' SQL Constant for an AS Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_AS As String = "AS"

			''' <summary>
			''' SQL Constant for an ASC Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_ASC As String = "ASC"

			''' <summary>
			''' SQL Constant for an BY Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_BY As String = "BY"

			''' <summary>
			''' SQL Constant for an CAST Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_CAST As String = "CAST"

			''' <summary>
			''' SQL Constant for an DESC Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_DESC As String = "DESC"

			''' <summary>
			''' SQL Constant for an FROM Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_FROM As String = "FROM"

			''' <summary>
			''' SQL Constant for an OF Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_OF As String = "OF"

			''' <summary>
			''' SQL Constant for an OR Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_OR As String = "OR"

			''' <summary>
			''' SQL Constant for an ORDER Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_ORDER As String = "ORDER"

			''' <summary>
			''' SQL Constant for an SELECT Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_SELECT As String = "SELECT"

			''' <summary>
			''' SQL Constant for an SCOPE Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_SCOPE As String = "SCOPE"

			''' <summary>
			''' SQL Constant for an TRAVERSAL Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_TRAVERSAL As String = "TRAVERSAL"

			''' <summary>
			''' SQL Constant for an WHERE Expression.
			''' </summary>
			''' <remarks></remarks>
			Public Const SQL_EXPRESSION_WHERE As String = "WHERE"

		#End Region

		#Region " Public Properties "

			Public Overloads ReadOnly Property Connection() As SqlConnection
				Get
					If String.IsNullOrEmpty(Source) Then _
						Throw New ArgumentNullException("Source Cannot be Nothing")
					Return Connection(Source)
				End Get
			End Property

			Public Overloads ReadOnly Property Connection( _
				ByVal connectionString As String _
			) As SqlConnection
				Get

					If Not Connections.ContainsKey(connectionString) Then _
						Connections.Add(connectionString, New SqlConnection(connectionString))
												
					If Connections(connectionString).State = ConnectionState.Closed Then _
						Connections(connectionString).Open()

					Return Connections(connectionString)

				End Get
			End Property

		#End Region

		#Region " Protected Shared Methods "

			' This method is used to attach array of SqlParameters to a SqlCommand.
			' This method will assign a value of DbNull to any parameter with a direction of
			' InputOutput and a value of null.
			' This behavior will prevent default values from being used, but
			' this will be the less common case than an intended pure output parameter (derived as InputOutput)
			' where the user provided no input value.
			' Parameters:
			' -command - The command to which the parameters will be added
			' -commandParameters - an array of SqlParameters to be added to command
			Protected Shared Sub AttachParameters( _
				ByRef command As SqlCommand, _
				ByVal ParamArray parameters As SqlParameter() _
			)

				If command Is Nothing Then Throw New ArgumentNullException("command")

				If Not parameters Is Nothing AndAlso parameters.Length > 0 Then

					For i As Integer = 0 To parameters.Length - 1

						If Not parameters(i) Is Nothing Then

							If parameters(i).Value Is Nothing AndAlso (parameters(i).Direction = ParameterDirection.InputOutput _
								OrElse parameters(i).Direction = ParameterDirection.Input) Then parameters(i).Value = DBNull.Value

							command.Parameters.Add(parameters(i))

						End If

					Next

				End If

			End Sub

			' This method assigns dataRow column values to an array of SqlParameters.
			' Parameters:
			' -commandParameters: Array of SqlParameters to be assigned values
			' -dataRow: the dataRow used to hold the stored procedure' s parameter values
			Protected Overloads Shared Sub AssignParameterValues( _
				ByVal values As DataRow, _
				ByVal ParamArray parameters As SqlParameter() _
			)

				If parameters Is Nothing OrElse parameters.Length = 0 OrElse values Is Nothing Then Exit Sub


				For i As Integer = 0 To parameters.Length - 1

					' Check the parameter name
					If String.IsNullOrEmpty(parameters(i).ParameterName) Then _
						Throw New Exception(String.Format("Please provide a valid parameter name on the parameter #{0}, " & _
						"the ParameterName property has the following value: ' {1}' .", i, parameters(i).ParameterName))

					If values.Table.Columns.IndexOf(parameters(i).ParameterName) >= 0 Then _
						parameters(i).Value = values(parameters(i).ParameterName)

				Next

			End Sub

			' This method assigns an array of values to an array of SqlParameters.
			' Parameters:
			' -commandParameters - array of SqlParameters to be assigned values
			' -array of objects holding the values to be assigned
			Protected Overloads Shared Sub AssignParameterValues( _
				ByVal values As Object(), _
				ByVal ParamArray parameters As SqlParameter(), _
			)

				If parameters Is Nothing OrElse parameters.Length = 0 OrElse values Is Nothing OrElse _
					values.Length = 0 Then Exit Sub

				' We must have the same number of values as we pave parameters to put them in
				If parameters.Length <> values.Length Then _
					Throw New ArgumentException("Parameter count does not match Value count.")

				For i As Integer = 0 To parameters.Length - 1

					' If the current array value derives from IDbDataParameter, then assign its Value property
					If TypeOf values(i) Is IDbDataParameter Then

						If (CType(values(i), IDbDataParameter).Value Is Nothing) Then

							parameters(i).Value = DBNull.Value

						Else

							parameters(i).Value = CType(values(i), IDbDataParameter).Value

						End If

					ElseIf (values(i) Is Nothing) Then

						parameters(i).Value = DBNull.Value

					Else

						parameters(i).Value = values(i)

					End If

				Next

			End Sub

			' This method opens (if necessary) and assigns a connection, transaction, command type and parameters
			' to the provided command.
			' Parameters:
			' -command - the SqlCommand to be prepared
			' -connection - a valid SqlConnection, on which to execute this command
			' -transaction - a valid SqlTransaction, or ' null'
			' -commandType - the CommandType (stored procedure, text, etc.)
			' -commandText - the stored procedure name or T-SQL command
			' -commandParameters - an array of SqlParameters to be associated with the command or ' null' if no parameters are required
			Protected Shared Sub PrepareCommand( _
				ByVal command As SqlCommand, _
				ByVal connection As SqlConnection, _
				ByVal transaction As SqlTransaction, _
				ByVal commandType As CommandType, _
				ByVal commandText As String, _
				ByRef mustCloseConnection As Boolean, _
				ByVal ParamArray parameters As SqlParameter() _
			)

				If command Is Nothing Then Throw New ArgumentNullException("command")
				If String.IsNullOrEmpty(commandText) Then Throw New ArgumentNullException("commandText")

				' If the provided connection is not open, we will open it
				If connection.State <> ConnectionState.Open Then

					connection.Open()
					mustCloseConnection = True

				Else

					mustCloseConnection = False

				End If

				With command

					.Connection = connection
					.CommandText = commandText
					.CommandType = commandType

				End With

				' If we were provided a transaction, assign it.
				If Not transaction Is Nothing Then

					If transaction.Connection Is Nothing Then Throw New ArgumentException( _
						"The transaction was rollbacked or commited, please provide an open transaction.", "transaction")

					command.Transaction = transaction

				End If

				' Attach the command parameters if they are provided
				If Not parameters Is Nothing AndAlso parameters.Length > 0 Then _
					AttachParameters(command, parameters)

			End Sub

		#End Region

		#Region " ExecuteReader Implementation "

			Public Overloads Shared Function ExecuteReader( _
				ByVal connection As SqlConnection, _
				ByVal transaction As SqlTransaction, _
				ByVal commandType As CommandType, _
				ByVal commandText As String, _
				ByVal commandParameters() As SqlParameter, _
				ByVal connectionOwnership As SqlConnectionOwnership _
			) As SqlDataReader

				If connection Is Nothing Then Throw New ArgumentNullException("connection")

				Dim mustCloseConnection As Boolean = False

				' Create a command and prepare it for execution
				Dim cmd As New SqlCommand

				Try
					' Create a reader
					Dim dataReader As SqlDataReader

					PrepareCommand(cmd, connection, transaction, commandType, commandText, mustCloseConnection, commandParameters)

					' Call ExecuteReader with the appropriate CommandBehavior
					If connectionOwnership = SqlConnectionOwnership.External Then

						dataReader = cmd.ExecuteReader()

					Else

						dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)

					End If

					' Detach the SqlParameters from the command object, so they can be used again
					Dim canClear As Boolean = True

					For i As Integer = 0 To cmd.Parameters.Count - 1

						If cmd.Parameters(i).Direction <> ParameterDirection.Input Then

							canClear = False
							Exit For

						End If

					Next

					If canClear Then cmd.Parameters.Clear()

					Return dataReader

				Catch ex As Exception

					If (mustCloseConnection) Then connection.Close()
					Throw ex

				End Try

			End Function

			Private Overloads Function ExecuteReader( _
				ByVal connectionString As String, _
				ByVal commandType As CommandType, _
				ByVal commandText As String _
			) As IDataReader

				Return ExecuteReader(Connection(connectionString), CType(Nothing, SqlTransaction), _
					commandType, commandText, CType(Nothing, SqlParameter()), SqlConnectionOwnership.External)

			End Function

			Private Overloads Function ExecuteReader( _
				ByVal commandType As CommandType, _
				ByVal commandText As String _
			) As IDataReader

				Return ExecuteReader(Connection, Nothing, commandType, commandText, _
					CType(Nothing, SqlParameter()), SqlConnectionOwnership.External)

			End Function

			Private Overloads Function ExecuteReader( _
				ByVal commandType As CommandType, _
				ByVal commandText As String, _
				ByVal ParamArray commandParameters As IDbDataParameter() _
			) As IDataReader

				Return ExecuteReader(Connection, Nothing, commandType, commandText, commandParameters, SqlConnectionOwnership.External)

			End Function

			Public Overloads Function ExecuteReader( _
				ByVal connectionString As String, _
				ByVal commandType As CommandType, _
				ByVal commandText As String, _
				ByVal ParamArray commandParameters As IDbDataParameter() _
			) As IDataReader

				Return ExecuteReader(Connection(connectionString), CType(Nothing, SqlTransaction), commandType, commandText, commandParameters, SqlConnectionOwnership.External)

			End Function

			Private Overloads Function ExecuteReader( _
				ByVal transaction As IDbTransaction, _
				ByVal commandType As CommandType, _
				ByVal commandText As String _
			) As IDataReader

				Return ExecuteReader(transaction, commandType, commandText, CType(Nothing, SqlParameter()))

			End Function

			Private Overloads Function ExecuteReader( _
				ByVal transaction As IDbTransaction, _
				ByVal commandType As CommandType, _
				ByVal commandText As String, _
				ByVal ParamArray commandParameters() As IDbDataParameter _
			) As IDataReader

				If transaction Is Nothing Then Throw New ArgumentNullException("transaction")

				If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then _
					Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")

				Return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External)

			End Function

		#End Region

		#Region " ExecuteNonQuery Implementation "

			Private Overloads Function ExecuteNonQuery( _
				ByVal commandType As CommandType, _
				ByVal commandText As String _
			) As Integer

				Return ExecuteNonQuery(commandType, commandText, CType(Nothing, SqlParameter()))

			End Function

			Private Overloads Function ExecuteNonQuery( _
				ByVal connectionString As String, _
				ByVal commandType As CommandType, _
				ByVal commandText As String _
			) As Integer

				Return ExecuteNonQuery(connectionString, commandType, commandText, CType(Nothing, SqlParameter()))

			End Function

			Private Overloads Function ExecuteNonQuery( _
				ByVal commandType As CommandType, _
				ByVal commandText As String, _
				ByVal ParamArray commandParameters As SqlParameter() _
			) As Integer

				' Create a command and prepare it for execution
				Dim cmd As New SqlCommand
				Dim retval As Integer
				Dim mustCloseConnection As Boolean = False

				PrepareCommand(cmd, Connection, CType(Nothing, SqlTransaction), commandType, _
					commandText, mustCloseConnection, commandParameters)

				' Finally, execute the command
				retval = cmd.ExecuteNonQuery()

				' Detach the SqlParameters from the command object, so they can be used again
				cmd.Parameters.Clear()

				If (mustCloseConnection) Then Connection.Close()

				Return retval

			End Function

			Public Overloads Function ExecuteNonQuery( _
				ByVal connectionString As String, _
				ByVal commandType As CommandType, _
				ByVal commandText As String, _
				ByVal ParamArray commandParameters As SqlParameter() _
			) As Integer

				' Create a command and prepare it for execution
				Dim cmd As New SqlCommand
				Dim retval As Integer
				Dim mustCloseConnection As Boolean = False

				PrepareCommand(cmd, Connection(connectionString), CType(Nothing, SqlTransaction), commandType, _
					commandText, mustCloseConnection, commandParameters)

				' Finally, execute the command
				retval = cmd.ExecuteNonQuery()

				' Detach the SqlParameters from the command object, so they can be used again
				cmd.Parameters.Clear()

				If (mustCloseConnection) Then Connection.Close()

				Return retval

			End Function

			Private Overloads Function ExecuteNonQuery( _
				ByVal transaction As IDbTransaction, _
				ByVal commandType As CommandType, _
				ByVal commandText As String _
			) As Integer

				Return ExecuteNonQuery(transaction, commandType, commandText, CType(Nothing, SqlParameter()))

			End Function

			Private Overloads Function ExecuteNonQuery( _
				ByVal transaction As IDbTransaction, _
				ByVal commandType As CommandType, _
				ByVal commandText As String, _
				ByVal ParamArray commandParameters As SqlParameter() _
			) As Integer

				If (transaction Is Nothing) Then Throw New ArgumentNullException("transaction")

				If Not (transaction Is Nothing) AndAlso (transaction.Connection Is Nothing) Then _
					Throw New ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction")

				' Create a command and prepare it for execution
				Dim cmd As New SqlCommand
				Dim retval As Integer
				Dim mustCloseConnection As Boolean = False

				PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, mustCloseConnection, commandParameters)

				' Finally, execute the command
				retval = cmd.ExecuteNonQuery()

				' Detach the SqlParameters from the command object, so they can be used again
				cmd.Parameters.Clear()

				Return retval

			End Function

		#End Region

		#Region " IDisposable Implementation "

			Public Sub Dispose() Implements System.IDisposable.Dispose

				For Each connectionString As String In Connections.Keys

					If Not Connections(connectionString).State = ConnectionState.Closed Then Connections(connectionString).Close()

				Next

			End Sub

		#End Region

	End Class

End Namespace