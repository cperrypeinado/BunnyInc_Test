Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization
Imports new

Public Class Index
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Const BioUrl = "https://torre.bio/api/bios/cperry85"

    Private Sub btnLinkedin_ServerClick(sender As Object, e As EventArgs) Handles btnLinkedin.ServerClick
        Response.Redirect("https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=78jtikm6oa90lk&redirect_uri=http%3A%2F%2Flocalhost%2FBunnyInc_Test%2Fcallback&state=9876asdqwdq54321e2w&scope=r_basicprofile")
    End Sub

    ''' <summary>
    ''' Evento clic del boton TorreBio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnTorre_ServerClick(sender As Object, e As EventArgs) Handles btnTorre.ServerClick
        'Se hará el llamado de TorreBio
        Try

            Dim webStream As Stream
            Dim webResponse = ""
            Dim request As HttpWebRequest
            Dim respons As HttpWebResponse
            Dim s As String

            request = CType(WebRequest.Create(BioUrl), HttpWebRequest)
            request.Method = "GET"
            request.Headers.Add("X-Auth-Token", "df19330b907ae11b6e28b3e32e8f1ef1998e6044-1-aRTHhOBQEpgUPnSiN+4TAwGmWuoEEk0L2apwA5E/nvraEsW7BgVcvrLzMta/g3WH8QqGmEuDpF0Z7O/iuMm90fmdkM727LWdSjZ6K7Gx32jfCWwtrkzd9+vNtSMnO0KLJyRidY/TkH79zPKQI1GmJLp20iCI8FUJ6K21iZfipC57JozJ1I477lDBCACOsfz0EKQ+OFjR7XzsmQ9U3CLaAplh+BERl1JgIEbh8DZWVPGUqAdBX+Um+cDdFvWRL5Ofy8lWuIMu8Bgfj8fm5d/f8juDDee0mV3XpQvd7Hp+5tRvdHMszSEqOQsmZvE5jkodscZakc/Ko00wHkCnl/WHaGTKh07gZL7pzLKXngWJLRa9sZhaF9f6Acivof0wz+Uq30BgG6FoOoUzznLEjXDnMcK9d+3nitRfcQi6vgtpMnrDY60y9LnBPSavJzlXvI6KcrYgkKO1bLuT7dpkwahyNA7BvleysiAfvd3qWLQSquKFfUcwP1gtIDJpLKXtYZTGk3ZiChH/Xb2/q3GyjQHfrSOzBopGn08eB7uoBDgkH2woqpvj/Uwh5sKyv9uhFQDfzs7mYWjjV88g451GmPSZtdsjiwjMSyWjpOCGJd6sc/7Y4gxuyeMOCbGYe3iqQfq4ekOau0hE5LypewNvCO9RbOzC6/hi9ttEtT0a3397")

            respons = CType(request.GetResponse(), HttpWebResponse)
            webStream = respons.GetResponseStream() ' Get Response
            Dim webStreamReader As New StreamReader(webStream)
            While webStreamReader.Peek >= 0
                webResponse = webStreamReader.ReadToEnd()
            End While

            MsgBox(webResponse.Length.ToString)


            Dim rawresp As String = webResponse '"{""id"":174543706,""first_name"":""Hamed"",""last_name"":""Ap"",""username"":""hamed_ap"",""type"":""private""}"

            Dim jss As New JavaScriptSerializer()
            Dim dict As Dictionary(Of String, String) = jss.Deserialize(Of Dictionary(Of String, String))(rawresp)

            s = dict("publicid")

            MsgBox(s)
        Catch ex As Exception

        End Try
    End Sub
End Class