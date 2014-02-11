#If TESTING Then

Imports Leviathan.Commands
Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

    <TestFixture()> _
    Public Class SuggestedCommandOutputTest

        <Test()> Public Sub CycleMethod()

            Dim strArray As String() = New String() _
                { _
                    "A", _ 
                    "B", _
                    "C", _
                    "D", _
                    "E", _
                    "F" _
                }

            Assert.AreEqual(strArray(0), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 0, 0)) _
            )

            Assert.AreEqual(strArray(1), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 0, 1)) _
            )

            Assert.AreEqual(strArray(2), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 1, 1)) _
            )

            Assert.AreEqual(strArray(0), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 1, -1)) _
            )

            Assert.AreEqual(strArray(5), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 2, 3)) _
            )

            Assert.AreEqual(strArray(0), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 0, 6)) _
            )

            Assert.AreEqual(strArray(0), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 0, 12)) _
            )

            Assert.AreEqual(strArray(1), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 0, 13)) _
            )

            Assert.AreEqual(strArray(0), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 0, -6)) _
            )

            Assert.AreEqual(strArray(0), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 0, -12)) _
            )

            Assert.AreEqual(strArray(5), _
                strArray(SuggestedCommandOutput.CalculateCycleShift(strArray.Length, 0, -13)) _
            )

        End Sub

    End Class
End Namespace

#End If