// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Library1.fs"
#r "System.Transactions"
open Deadfile.Fs

// Define your library scripting code here
open System
open System.Data.Sql
open System.Data.SqlClient

let connectionString = "Server=MRB-PC\SQLEXPRESS;Database=DeadfileTestDb;Trusted_Connection=True"
let connection = new SqlConnection(connectionString)
connection.Open()
let command = new SqlCommand("SELECT [ClientId],[FirstName],[LastName] from Clients", connection)
let reader = command.ExecuteReader()
reader.Read()
let clientId = reader.GetInt32(0)
let firstName = reader.GetString(1)
let lastName = reader.GetString(2)

let date = new DateTime(2016,8,28)
date.ToString("o")
date.ToString("dd MMM yyyy")

