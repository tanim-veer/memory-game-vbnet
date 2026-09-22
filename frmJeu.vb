Imports AxWMPLib
Public Class frmJeu
    Private tempsRestant As Integer
    Public DureeChoisie As Integer = 60
    Private WithEvents pauseTimer As Timer = New Timer()
    Private estActif As Boolean = True
    Private aleatoire As New Random()
    Private cartesDevoilees As New Dictionary(Of PictureBox, String)()
    Private cartesRetournees As New List(Of PictureBox)()
    Private valeurChaineActuelle As String = ""
    Private compteChaine As Integer = 0
    Private nbPairesTrouvees As Integer = 0
    Private indicesUtilises As Integer = 0
    Private dureeIndice As Integer = 1000
    Private boutonsIndice As New List(Of PictureBox)()
    Public ThemeSelectionne As String
    Private liensImages As New List(Of String)

    Private Sub frmJeu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tempsRestant = DureeChoisie
        lblTemps.Text = tempsRestant.ToString()

        If ThemeSelectionne = "Cinema" Then
            liensImages = New List(Of String) From {
                "buster.jpg", "star.jpg", "Marvel.jpg", "DC.jpg", "WB.jpg"
            }.Select(Function(f) IO.Path.Combine(Application.StartupPath, "Images", f)).ToList()
        ElseIf ThemeSelectionne = "FastFood" Then
            liensImages = New List(Of String) From {
                "bk.jpg", "domi.jpg", "hut.jpg", "pepe.jpg", "kfc.jpg"
            }.Select(Function(f) IO.Path.Combine(Application.StartupPath, "Images2", f)).ToList()
        Else
            MessageBox.Show("Erreur : Aucun thème choisi", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            frmThemes.Show()
            Exit Sub
        End If

        Timer1.Interval = 1000
        Timer1.Start()
        lblTemps.Text = tempsRestant.ToString()
        lblNom.Text = frmAccueil.tbNom.Text

        Dim valeurs As New List(Of String)
        For Each lien In liensImages
            For j = 1 To 4
                valeurs.Add(lien)
            Next
        Next

        Dim pictureBoxes As New List(Of PictureBox) From {
            PictureBox1, PictureBox2, PictureBox3, PictureBox4, PictureBox5,
            PictureBox6, PictureBox7, PictureBox8, PictureBox9, PictureBox10,
            PictureBox11, PictureBox12, PictureBox13, PictureBox14, PictureBox15,
            PictureBox16, PictureBox17, PictureBox18, PictureBox19, PictureBox20
        }

        For Each pictureBox In pictureBoxes
            Dim index As Integer = aleatoire.Next(valeurs.Count)
            cartesDevoilees.Add(pictureBox, valeurs(index))
            valeurs.RemoveAt(index)
            pictureBox.BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Images\Cer.jpg"))
            pictureBox.BackgroundImageLayout = ImageLayout.Stretch
            pictureBox.Image = Nothing
            AddHandler pictureBox.Click, AddressOf Clic_Carte
        Next

        AxWindowsMediaPlayer1.URL = IO.Path.Combine(Application.StartupPath, "music\disco.mp3")
        AxWindowsMediaPlayer1.settings.setMode("loop", True)
        AxWindowsMediaPlayer1.Ctlcontrols.play()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If tempsRestant > 0 Then
            tempsRestant -= 1
            lblTemps.Text = tempsRestant.ToString()
        Else
            Timer1.Stop()
            lblTemps.Text = "0"

            Dim tempsUtilise As Integer = DureeChoisie - tempsRestant
            If tempsUtilise < 0 Then tempsUtilise = 0
            If tempsUtilise > DureeChoisie Then tempsUtilise = DureeChoisie

            MessageBox.Show("Temps écoulé ! Paires trouvées : " & nbPairesTrouvees, "Fin", MessageBoxButtons.OK, MessageBoxIcon.Information)
            AxWindowsMediaPlayer1.Ctlcontrols.stop()

            Dim scoreForm As New frmScores()
            scoreForm.AjouterScore(frmAccueil.tbNom.Text, nbPairesTrouvees, tempsUtilise, DureeChoisie)
            scoreForm.ShowDialog()

            Me.Close()
            frmAccueil.Show()
        End If
    End Sub

    Private Sub PauseTimer_Tick(sender As Object, e As EventArgs) Handles pauseTimer.Tick
        pauseTimer.Stop()
        If boutonsIndice.Count > 0 Then
            For Each pictureBox In boutonsIndice
                pictureBox.Image = Nothing
            Next
            boutonsIndice.Clear()
        Else
            For Each pictureBox In cartesRetournees
                pictureBox.Image = Nothing
            Next
            cartesRetournees.Clear()
            compteChaine = 0
            valeurChaineActuelle = ""
        End If

        For Each pictureBox In cartesDevoilees.Keys
            If pictureBox.Image Is Nothing Then
                pictureBox.Enabled = True
            End If
        Next

        If estActif Then
            Timer1.Start()
        End If
    End Sub

    Private Sub Clic_Carte(sender As Object, e As EventArgs)
        Dim carte As PictureBox = CType(sender, PictureBox)

        If Not carte.Enabled Or cartesRetournees.Contains(carte) Then
            Exit Sub
        End If

        Dim cheminImage As String = cartesDevoilees(carte)
        carte.Image = Image.FromFile(cheminImage)
        carte.SizeMode = PictureBoxSizeMode.StretchImage

        cartesRetournees.Add(carte)

        If compteChaine = 0 Then
            valeurChaineActuelle = cheminImage
            compteChaine = 1
        ElseIf cheminImage = valeurChaineActuelle Then
            compteChaine += 1

            If compteChaine = 4 Then
                For Each c In cartesRetournees
                    c.Enabled = False
                Next
                cartesRetournees.Clear()
                compteChaine = 0
                valeurChaineActuelle = ""
                nbPairesTrouvees += 1

                VerifierFinPartie()
            End If
        Else
            For Each c In cartesDevoilees.Keys
                c.Enabled = False
            Next

            pauseTimer.Interval = 300
            pauseTimer.Start()
        End If
    End Sub

    Private Sub btnPause_Click(sender As Object, e As EventArgs) Handles btnPause.Click
        If estActif Then
            btnPause.Text = "Reprendre"
            estActif = False
            Timer1.Stop()
            AxWindowsMediaPlayer1.Ctlcontrols.pause()
            For Each pictureBox In cartesDevoilees.Keys
                pictureBox.Enabled = False
            Next
        Else
            btnPause.Text = "Pause"
            estActif = True
            Timer1.Start()
            AxWindowsMediaPlayer1.Ctlcontrols.play()
            For Each pictureBox In cartesDevoilees.Keys
                If pictureBox.Image Is Nothing Then
                    pictureBox.Enabled = True
                End If
            Next
        End If
    End Sub

    Private Sub btnAbandonner_Click(sender As Object, e As EventArgs) Handles btnAbandonner.Click
        If MsgBox("Veux-tu abandonner ?", vbYesNo) = vbYes Then
            Me.Close()
            frmAccueil.Show()
            frmAccueil.AxWindowsMediaPlayer1.Ctlcontrols.play()
            Timer1.Stop()
        End If
    End Sub

    Private Sub btnIndice_Click_1(sender As Object, e As EventArgs) Handles btnIndice.Click
        Dim pictureBoxesActifs = cartesDevoilees.Keys.Where(Function(p) p.Enabled).ToList()
        If pictureBoxesActifs.Count < 4 Then
            MsgBox("Pas assez de cartes libres.", MsgBoxStyle.Exclamation, "Indice")
            Exit Sub
        End If
        If cartesRetournees.Count > 0 Then
            MsgBox("Termine ton coup avant l'indice.", MsgBoxStyle.Exclamation, "Indice")
            Exit Sub
        End If
        If indicesUtilises >= 3 Then
            MsgBox("Tous les indices sont utilisés.", MsgBoxStyle.Exclamation, "Indice")
            Exit Sub
        End If

        indicesUtilises += 1

        For Each p In cartesDevoilees.Keys
            p.Enabled = False
        Next

        Dim aleatoire As New Random()
        Dim pictureBoxesAleatoires = pictureBoxesActifs.OrderBy(Function(p) aleatoire.Next()).Take(4).ToList()

        boutonsIndice.Clear()
        For Each p In pictureBoxesAleatoires
            p.Image = Image.FromFile(cartesDevoilees(p))
            p.SizeMode = PictureBoxSizeMode.StretchImage
            boutonsIndice.Add(p)
        Next

        Timer1.Stop()
        pauseTimer.Interval = dureeIndice
        pauseTimer.Start()
    End Sub

    Private Sub VerifierFinPartie()
        If tempsRestant <= 0 OrElse nbPairesTrouvees = 5 Then
            Timer1.Stop()
            AxWindowsMediaPlayer1.Ctlcontrols.stop()

            Dim tempsUtilise As Integer = DureeChoisie - tempsRestant

            If tempsUtilise < 0 Then tempsUtilise = 0
            If tempsUtilise > DureeChoisie Then tempsUtilise = DureeChoisie

            Dim scoreForm As New frmScores()
            scoreForm.AjouterScore(frmAccueil.tbNom.Text, nbPairesTrouvees, tempsUtilise, DureeChoisie)
            scoreForm.ShowDialog()
            Me.Close()
            frmAccueil.Show()
        End If
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        nbPairesTrouvees = 0
        indicesUtilises = 0
        tempsRestant = DureeChoisie
        lblTemps.Text = tempsRestant.ToString()

        Dim valeurs As New List(Of String)
        For Each lien In liensImages
            For j = 1 To 4
                valeurs.Add(lien)
            Next
        Next

        Dim pictureBoxes As New List(Of PictureBox) From {
            PictureBox1, PictureBox2, PictureBox3, PictureBox4, PictureBox5,
            PictureBox6, PictureBox7, PictureBox8, PictureBox9, PictureBox10,
            PictureBox11, PictureBox12, PictureBox13, PictureBox14, PictureBox15,
            PictureBox16, PictureBox17, PictureBox18, PictureBox19, PictureBox20
        }

        cartesDevoilees.Clear()
        cartesRetournees.Clear()
        boutonsIndice.Clear()
        compteChaine = 0
        valeurChaineActuelle = ""

        For Each pictureBox In pictureBoxes
            Dim index As Integer = aleatoire.Next(valeurs.Count)
            cartesDevoilees.Add(pictureBox, valeurs(index))
            valeurs.RemoveAt(index)

            pictureBox.BackgroundImage = Image.FromFile(IO.Path.Combine(Application.StartupPath, "Images\Cer.jpg"))
            pictureBox.BackgroundImageLayout = ImageLayout.Stretch
            pictureBox.Image = Nothing
            pictureBox.Enabled = True
        Next

        Timer1.Stop()
        Timer1.Start()
        estActif = True
    End Sub
End Class
