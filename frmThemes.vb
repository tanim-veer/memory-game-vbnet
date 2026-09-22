Imports AxWMPLib
Public Class frmThemes
    Private themeSelectionne As String = ""

    Private Sub btnCinema_Click(sender As Object, e As EventArgs) Handles btnCinema.Click
        themeSelectionne = "Cinema"
        PictureBox1.Image = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Images\Cine.jpg"))
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub

    Private Sub btnFastFood_Click(sender As Object, e As EventArgs) Handles btnOptions.Click
        themeSelectionne = "FastFood"
        PictureBox1.Image = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Images2\Fast.jpg"))
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub

    Private Sub btnQuitter_Click(sender As Object, e As EventArgs) Handles btnQuitter.Click
        Me.Close()
        frmAccueil.Show()
        frmAccueil.AxWindowsMediaPlayer1.Ctlcontrols.play()
    End Sub

    Private Sub btnJouer_Click(sender As Object, e As EventArgs) Handles btnJouer.Click
        Dim dureeSelectionnee As Integer = 60
        Dim jeuForm As New frmJeu()

        If themeSelectionne = "" Then
            MessageBox.Show("Veuillez sélectionner un thème avant de jouer.", "Thème manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If rb30Sec.Checked Then
            dureeSelectionnee = 30
        ElseIf rb90Sec.Checked Then
            dureeSelectionnee = 90
        End If

        jeuForm.ThemeSelectionne = themeSelectionne
        jeuForm.DureeChoisie = dureeSelectionnee
        jeuForm.Show()
        Me.Hide()
    End Sub

    Private Sub frmThemes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim cheminImage As String = IO.Path.Combine(Application.StartupPath, "Images\cinema5.jpg")

        Me.BackgroundImage = Image.FromFile(cheminImage)
        Me.BackgroundImageLayout = ImageLayout.Stretch
        AxWindowsMediaPlayer1.URL = IO.Path.Combine(Application.StartupPath, "music\song.mp3")
        AxWindowsMediaPlayer1.settings.setMode("loop", False)
        AxWindowsMediaPlayer1.Ctlcontrols.play()
    End Sub

End Class
