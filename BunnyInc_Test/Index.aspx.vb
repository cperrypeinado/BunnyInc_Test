Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization


Public Class Index
    Inherits System.Web.UI.Page

    Private strClienId As String = "78jtikm6oa90lk"
    Private strClientSecret As String = "Jfs3XH5JKQPmH762"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

            If Not Request.QueryString Is Nothing Then
                Dim code As String


                code = Request.QueryString("code")
                imgFotoPerfil.Visible = False


                If code <> "" Then 'Se hace el paso dos para el llamado de linkedin
                    If Session("PersonID") <> "" Then

                        Dim webStream As Stream
                        Dim webResponse = ""
                        Dim request As HttpWebRequest
                        Dim respons As HttpWebResponse
                        Dim s As String

                        request = CType(WebRequest.Create("https://www.linkedin.com/oauth/v2/accessToken"), HttpWebRequest)
                        request.Method = "POST"
                        request.ContentType = "application/x-www-form-urlencoded"

                        Dim poststring As String = "grant_type=authorization_code&code=" & code & "&redirect_uri=http://localhost:4311/Index.aspx&client_id=" & strClienId & "&client_secret=" & strClientSecret
                  
                        request.ContentLength = poststring.Length

                        Dim requestWriter As New StreamWriter(request.GetRequestStream())
                        requestWriter.Write(poststring)
                        requestWriter.Close()

                        respons = CType(request.GetResponse(), HttpWebResponse)
                        webStream = respons.GetResponseStream() ' Get Response
                        Dim webStreamReader As New StreamReader(webStream)
                        While webStreamReader.Peek >= 0
                            webResponse = webStreamReader.ReadToEnd()
                        End While

                        'Obtener el código de acceso
                        Dim rawresp As String = webResponse '"{""id"":174543706,""first_name"":""Hamed"",""last_name"":""Ap"",""username"":""hamed_ap"",""type"":""private""}"

                        Dim jss As New JavaScriptSerializer()
                        Dim accessCode As Dictionary(Of String, String) = jss.Deserialize(Of Dictionary(Of String, String))(rawresp)

                        s = accessCode("access_token")

                        request = Nothing
                        request = CType(WebRequest.Create("https://api.linkedin.com/v1/people/~?format=json"), HttpWebRequest)
                        request.Headers.Add("Authorization", "Bearer " & s)
                        request.Method = "GET"

                        respons = CType(request.GetResponse(), HttpWebResponse)
                        webStream = respons.GetResponseStream() ' Get Response
                        webStreamReader = Nothing
                        webStreamReader = New StreamReader(webStream)
                        While webStreamReader.Peek >= 0
                            webResponse = webStreamReader.ReadToEnd()
                        End While


                        Dim prfNombre As String
                        Dim pfrHeadline As String

                        prfNombre = webResponse.Split(":")(1).Split(",")(0).Replace(Chr(34), "") & " " & webResponse.Split(":")(4).Split(",")(0).Replace(Chr(34), "")
                        pfrHeadline = webResponse.Split(":")(2).Split(",")(0).Replace(Chr(34), "")

                        request = Nothing
                        request = CType(WebRequest.Create("https://api.linkedin.com/v1/people/~:(id,num-connections,picture-url)?format=json"), HttpWebRequest)
                        request.Headers.Add("Authorization", "Bearer " & s)
                        request.Method = "GET"

                        respons = CType(request.GetResponse(), HttpWebResponse)
                        webStream = respons.GetResponseStream() ' Get Response
                        webStreamReader = Nothing
                        webStreamReader = New StreamReader(webStream)
                        While webStreamReader.Peek >= 0
                            webResponse = webStreamReader.ReadToEnd()
                        End While

                        Dim prfPicture As String
                        prfPicture = webResponse.Split(",")(2).Replace(Chr(34), "")
                        prfPicture = prfPicture.Split(":")(1).Replace(Chr(34), "") & ":" & prfPicture.Split(":")(2).Replace(Chr(34), "").Replace("}", "")
                        ' Dim profideDetLnk As Dictionary(Of String, String) = jss.Deserialize(Of Dictionary(Of String, String))(webResponse.ToString())
                        imgFotoPerfil.Src = prfPicture
                        imgFotoPerfil.Visible = True

                        lblProfle.InnerText = prfNombre
                        lblHead.InnerText = pfrHeadline

                        If getInfoTorre(Session("PersonID")) Then
                            txtPersonID.Visible = False
                            btnLinkedin.Visible = False
                            tblIngreso.Visible = False
                            lblHead.Visible = True
                        Else
                            txtPersonID.Visible = True
                            btnLinkedin.Visible = True
                            tblIngreso.Visible = True

                            imgFotoPerfil.Src = ""
                            imgFotoPerfil.Visible = False

                            lblProfle.Visible = False
                            lblHead.Visible = False

                            Response.Write("<script>alert('TorreBio profile dont found.');</script>")
                        End If
                    Else

                        Response.Write("<script>alert('Communication error.');</script>")
                    End If
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message & " - ")
        End Try
    End Sub

    Private Const BioUrl = "https://torre.bio/api/bios/"

    Private Sub btnLinkedin_ServerClick(sender As Object, e As EventArgs) Handles btnLinkedin.ServerClick
        If txtPersonID.Value = "" Then
            Response.Write("<script>alert('Insert your Torre Bio ID');</script>")
        Else
            Response.Write("<script>window.open('https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=78jtikm6oa90lk&redirect_uri=http://localhost:4311/Index.aspx&state=9876asdqwdq54321e2w&scope=r_basicprofile');</script>")
            Session("PersonID") = txtPersonID.Value.ToString()
        End If
    End Sub

    Private Function getInfoTorre(ByVal strPersonID) As Boolean
        Try
            If strPersonID <> "" Then
                Dim webStream As Stream
                Dim webResponse = ""
                Dim request As HttpWebRequest
                Dim respons As HttpWebResponse
                Dim s As String

                request = CType(WebRequest.Create(BioUrl & strPersonID), HttpWebRequest)
                request.Method = "GET"
                request.Headers.Add("X-Auth-Token", "df19330b907ae11b6e28b3e32e8f1ef1998e6044-1-aRTHhOBQEpgUPnSiN+4TAwGmWuoEEk0L2apwA5E/nvraEsW7BgVcvrLzMta/g3WH8QqGmEuDpF0Z7O/iuMm90fmdkM727LWdSjZ6K7Gx32jfCWwtrkzd9+vNtSMnO0KLJyRidY/TkH79zPKQI1GmJLp20iCI8FUJ6K21iZfipC57JozJ1I477lDBCACOsfz0EKQ+OFjR7XzsmQ9U3CLaAplh+BERl1JgIEbh8DZWVPGUqAdBX+Um+cDdFvWRL5Ofy8lWuIMu8Bgfj8fm5d/f8juDDee0mV3XpQvd7Hp+5tRvdHMszSEqOQsmZvE5jkodscZakc/Ko00wHkCnl/WHaGTKh07gZL7pzLKXngWJLRa9sZhaF9f6Acivof0wz+Uq30BgG6FoOoUzznLEjXDnMcK9d+3nitRfcQi6vgtpMnrDY60y9LnBPSavJzlXvI6KcrYgkKO1bLuT7dpkwahyNA7BvleysiAfvd3qWLQSquKFfUcwP1gtIDJpLKXtYZTGk3ZiChH/Xb2/q3GyjQHfrSOzBopGn08eB7uoBDgkH2woqpvj/Uwh5sKyv9uhFQDfzs7mYWjjV88g451GmPSZtdsjiwjMSyWjpOCGJd6sc/7Y4gxuyeMOCbGYe3iqQfq4ekOau0hE5LypewNvCO9RbOzC6/hi9ttEtT0a3397")

                respons = CType(request.GetResponse(), HttpWebResponse)
                webStream = respons.GetResponseStream() ' Get Response
                Dim webStreamReader As New StreamReader(webStream)
                While webStreamReader.Peek >= 0
                    webResponse = webStreamReader.ReadToEnd()
                End While

                Dim rawresp As String = webResponse '"{""id"":174543706,""first_name"":""Hamed"",""last_name"":""Ap"",""username"":""hamed_ap"",""type"":""private""}"

                lblHead.InnerHtml = lblHead.InnerHtml & "<br />" & rawresp.Split(":")(9).Split(",")(0).Replace(Chr(34), "")
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    
End Class

