Imports AxWMPLib
Public Class frmAccueil
    Public Structure Score
        Public Prenom As String
        Public TempsUtilise As Integer
        Public NbPaires As Integer
    End Structure

    Public ListeScores As New List(Of Score)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnJouer.Click
        If tbNom.Text.Length < 3 Then
            MsgBox("Tu n'as pas assez de lettres dans ton prénom.")
        Else
            frmThemes.Show()
            AxWindowsMediaPlayer1.Ctlcontrols.stop()
            Me.Hide()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnOptions.Click
        MsgBox("Jeu de Memory - Variante solo" & vbCrLf & vbCrLf &
               "But : Retrouver le maximum de carrés (4 cartes identiques) en 60 secondes." & vbCrLf & vbCrLf &
               "Déroulement :" & vbCrLf &
               "- Le joueur saisit son nom (minimum 3 caractères) pour commencer." & vbCrLf &
               "- 20 cartes sont affichées (4 lignes x 5 colonnes), dans un ordre aléatoire." & vbCrLf &
               "- Le joueur clique pour retourner les cartes et essayer de former des carrés." & vbCrLf &
               "- Si 4 cartes identiques sont trouvées, elles sont désactivées." & vbCrLf &
               "- En cas d'erreur, les cartes sont retournées face cachée." & vbCrLf &
               "- Le jeu s’arrête quand le temps est écoulé ou tous les carrés sont trouvés." & vbCrLf &
               "- Un bouton permet d’abandonner la partie après confirmation." & vbCrLf & vbCrLf &
               "Fin de partie :" & vbCrLf &
               "- Le jeu affiche le nombre de carrés trouvés et le temps utilisé." & vbCrLf &
               "- Les statistiques du joueur sont enregistrées.")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnScores.Click
        frmScores.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles btnQuitter.Click
        If MsgBox("Veux-tu quitter ?", vbYesNo) = vbYes Then
            Me.Close()
        End If
    End Sub

    Private Sub frmAccueil_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim cheminImage As String = IO.Path.Combine(Application.StartupPath, "Images\cinema3.jpg")

        Me.BackgroundImage = Image.FromFile(cheminImage)
        Me.BackgroundImageLayout = ImageLayout.Stretch

        btnJouer.Enabled = False

        AxWindowsMediaPlayer1.URL = IO.Path.Combine(Application.StartupPath, "music\coral.mp3")
        AxWindowsMediaPlayer1.settings.setMode("loop", True)
        AxWindowsMediaPlayer1.Ctlcontrols.play()
    End Sub
    Private Sub tbNom_TextChanged(sender As Object, e As EventArgs) Handles tbNom.TextChanged
        btnJouer.Enabled = (tbNom.Text.Length >= 3)
    End Sub
End Class
