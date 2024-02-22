Imports System.IO
Public Class Form1
    Structure card
        Public cardBack As Image
        Public image As Image
        Public button As Button
    End Structure

    Dim iconArray() As Image = {My.Resources._1, My.Resources._7, My.Resources._20, My.Resources._25,
        My.Resources._28, My.Resources._40, My.Resources._44, My.Resources._18, My.Resources._4}
    Dim cardBackImage As Image = My.Resources._6
    Dim buttonArray(15) As Button
    Dim cardArray(15) As card

    Dim randomGen As New Random
    Dim randomNumber As Integer
    Dim numberOfTurns As Integer
    Dim winCon As Integer = 0 'when this reaches 8 the gameWon boolean is set to true

    Dim outFile As StreamWriter
    Dim inFile As StreamReader

    Dim cardTwo As Boolean = False
    Dim gameWon As Boolean = False
    Dim tempImage As Image
    Dim tempButton As Button
    Dim tempScore As Integer
    Dim scoreName As String

    Dim topScore As String

    Dim alist As New ArrayList
    Dim filePath As String = "C:\Users\Rod\Desktop\scores.txt"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'pulls the scores from the txt file into the array list
        If File.Exists(filePath) Then

            inFile = File.OpenText(filePath)

            Do Until inFile.Peek = -1
                alist.Add(Integer.Parse(inFile.ReadLine))
            Loop
            inFile.Close()

            If File.ReadAllText(filePath).Length <> 0 Then
                alist.Sort()
                topScore = alist(0)
            End If
        Else
            MessageBox.Show("File not found")
        End If
        inFile.Close()

        'loads the buttons into the button array
        buttonArray(0) = Button1
        buttonArray(1) = Button2
        buttonArray(2) = Button3
        buttonArray(3) = Button4
        buttonArray(4) = Button5
        buttonArray(5) = Button6
        buttonArray(6) = Button7
        buttonArray(7) = Button8
        buttonArray(8) = Button9
        buttonArray(9) = Button10
        buttonArray(10) = Button11
        buttonArray(11) = Button12
        buttonArray(12) = Button13
        buttonArray(13) = Button14
        buttonArray(14) = Button15
        buttonArray(15) = Button16

        'create and shuffle cards
        createCards()
        shuffleCards(cardArray)
        highScoreLabel.Text = topScore

        'gives every button the same click address
        For i As Integer = 0 To buttonArray.Length - 1
            AddHandler buttonArray(i).Click, AddressOf dynamicClick
        Next

    End Sub

    Private Sub dynamicClick(sender As Object, ByVal e As EventArgs)
        Dim btn As Button = CType(sender, Button)

        Dim buttonIndex = Controls.IndexOf(btn) 'gets the index of the button being clicked
        resultLabel.Text = ""

        btn.BackgroundImage = cardArray(buttonIndex - 17).image 'sets the index to the cardArray index

        ' if statement checks if the first card or second card has been clicked
        If cardTwo = False Then
            tempImage = btn.BackgroundImage
            tempButton = btn
            cardTwo = True
        Else
            'checks if cards match, if matched, adds a win condition counter
            If tempImage Is btn.BackgroundImage Then
                winCon += 1
                cardTwo = True
                tempImage = Nothing
                btn.Enabled = False
                tempButton.Enabled = False
                resultLabel.Text = "Match"

                Application.DoEvents() 'added to create a pause
                Threading.Thread.Sleep(500) 'before cards are removed from the board


                btn.Visible = False 'this bit of code can be enabled or disabled
                tempButton.Visible = False 'depending if you want the matched pairs to remain on the screen or be removed

                'if win condition is met, saves the score to the array list and saves it to the txt file
                If winCon = 8 Then
                    gameWon = True
                    winLabel.Text = "All matches have been found"
                    numberOfTurns += 1
                    turnsLabel.Text = numberOfTurns

                    tempScore = numberOfTurns
                    youWinLabel.Visible = True

                    alist.Add(tempScore)
                    File.WriteAllText(filePath, "")
                    outFile = File.AppendText(filePath)

                    For Each currentElement As String In alist
                        outFile.WriteLine(currentElement)
                    Next

                    highScoreLabel.Text = topScore
                    outFile.Close()

                End If
            Else
                'if cards are not matched adds a small delay and then flips the cards back
                cardTwo = False
                resultLabel.Text = "No Match"

                Application.DoEvents() 'is this bad coding practice?
                Threading.Thread.Sleep(500) 'added to create a pause before cards are flipped over

                cardFlip(btn)
                cardFlip(tempButton)
                tempImage = Nothing

            End If

            If gameWon = False Then
                numberOfTurns += 1
                turnsLabel.Text = numberOfTurns
            End If

        End If

    End Sub

    Private Sub shuffleCards(cards As card())
        Dim j As Integer
        Dim tempCard As card

        For i As Integer = cards.Length - 1 To 0 Step -1
            j = randomGen.Next(0, i + 1)
            tempCard = cards(i)
            cards(i) = cards(j)
            cards(j) = tempCard
        Next i

    End Sub

    Private Sub createCards()
        'to create the cards,add the buttons to the cardArray
        For i As Integer = 0 To cardArray.Length - 1
            cardArray(i).button = buttonArray(i)
            cardArray(i).cardBack = cardBackImage
        Next
        'add the images to the cardArray
        For j As Integer = 0 To 7
            cardArray(j).image = iconArray(j)
        Next
        'add the second set of images to the cardArray
        For k As Integer = 0 To 7
            cardArray(k + 8).image = iconArray(k)
        Next
    End Sub

    Private Sub cardFlip(btn As Button)
        'added a timer to handle the card flip back to the card back image
        flipTimer.Start()
        btn.BackgroundImage = cardBackImage
        flipTimer.Stop()
    End Sub

    Private Sub newButton_Click(sender As Object, e As EventArgs) Handles newButton.Click

        'resets the game screen

        For i As Integer = 1 To buttonArray.Length - 1
            buttonArray(i).BackgroundImage = cardBackImage
            buttonArray(i).Enabled = True
            buttonArray(i).Visible = True
        Next

        shuffleCards(cardArray)

        'reset labels and variables
        gameWon = False
        numberOfTurns = 0
        turnsLabel.Text = 0
        resultLabel.Text = "New Game Created"
        winLabel.Text = ""
        youWinLabel.Visible = False

    End Sub

    Private Sub highScoreButton_Click(sender As Object, e As EventArgs) Handles highScoreButton.Click

        'sort the array list then create a string to add to the high scores label
        alist.Sort()

        Dim results As String = ""

        For i As Integer = 0 To alist.Count - 1
            results += alist(i) & Environment.NewLine
            highScoresLabel.Text = results
        Next
    End Sub

End Class
