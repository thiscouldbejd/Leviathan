#If TESTING Then

Imports System.Configuration
Imports System.Drawing
Imports NUnit.Framework

Namespace Testing

       <Flags()> _
        Public Enum TestEnum
            One = 1
            Two = 2
            Four = 4
            Eight = 8
        End Enum

        <TestFixture()> _
        Public Class ParsingFromStringTest

            <Test()> Public Sub CheckColourParsing()

                Dim parser As New FromString()

                Dim parseSuccess As Boolean = False
                Dim value As Color = parser.Parse("Red", parseSuccess, GetType(Color))

                Assert.IsNotNull(value)
                Assert.IsTrue(parseSuccess)
                Assert.AreEqual(Color.Red, value)
            End Sub

            <Test()> Public Sub CheckEnumParsing()

                Dim parser As New FromString()

                Dim parseSuccess As Boolean = False
                Dim value As TestEnum

                value = parser.Parse("One", parseSuccess, GetType(TestEnum))

                Assert.AreEqual(TestEnum.One, value)

                value = parser.Parse("One;Four", parseSuccess, GetType(TestEnum))
                Assert.AreEqual(TestEnum.One Or TestEnum.Four, value)
                Assert.AreEqual(5, value)
            End Sub

            <Test()> Public Sub CheckFontParsing()

                Dim parser As New FromString()

                Dim parseSuccess As Boolean = False
                Dim value As FontFamily

                value = parser.Parse("Arial", parseSuccess, GetType(FontFamily))

                Assert.AreEqual(New FontFamily("Arial"), value)

            End Sub

            <Test()> Public Sub CheckGuidParsing_1()

                Dim parser As New FromString()

                Dim parseSuccess As Boolean = False
                Dim nGuid As Guid = Guid.NewGuid

                Dim value As Guid = parser.Parse(nGuid.ToString, parseSuccess, GetType(Guid))

                Assert.AreEqual(nGuid, value)

            End Sub

            <Test()> Public Sub CheckGuidParsing_2()

                Dim parser As New FromString()

                Dim testGuid As New Guid("d1620cfe-5347-4984-9be9-63243fd7a981")

                Dim success_1 As Boolean = False
                Assert.AreEqual(testGuid, _
                    parser.Parse("d1620cfe-5347-4984-9be9-63243fd7a981", success_1, GetType(Guid)))
                Assert.IsTrue(success_1)

                Dim success_2 As Boolean = False
                Assert.AreEqual(testGuid, _
                    parser.Parse("{d1620cfe-5347-4984-9be9-63243fd7a981}", success_2, GetType(Guid)))
                Assert.IsTrue(success_2)

                Dim success_3 As Boolean = False
                Assert.AreEqual(testGuid, _
                    parser.Parse("d1620cfe534749849be963243fd7a981", success_3, GetType(Guid)))
                Assert.IsTrue(success_3)

            End Sub

            <Test()> Public Sub CheckTypeParsing()

                Dim parser As New FromString()

                Dim parseSuccess As Boolean = False

                Dim value As Type = parser.Parse("ParsingFromStringTest", parseSuccess, GetType(System.Type))

                Assert.AreEqual(GetType(ParsingFromStringTest), value)

            End Sub

            <Test()> Public Sub CheckTimeSpanParsing_1()

                Dim parser As New FromString()

                Dim success_1 As Boolean = False
                Assert.AreEqual(New TimeSpan(65, 0, 0, 0), _
                    parser.Parse("65days", success_1, GetType(TimeSpan)))
                Assert.IsTrue(success_1)

                Dim success_2 As Boolean = False
                Assert.AreEqual(New TimeSpan(1, 0, 0), _
                    parser.Parse("1hours", success_2, GetType(TimeSpan)))
                Assert.IsTrue(success_2)

                Dim success_3 As Boolean = False
                Assert.AreEqual(New TimeSpan(5, 0, 0), _
                    parser.Parse("5hours", success_3, GetType(TimeSpan)))
                Assert.IsTrue(success_3)

                Dim success_4 As Boolean = False
                Assert.AreEqual(New TimeSpan(0, 6, 0), _
                    parser.Parse("6mins", success_4, GetType(TimeSpan)))
                Assert.IsTrue(success_4)

                Dim success_5 As Boolean = False
                Assert.AreEqual(New TimeSpan(0, 0, 30), _
                    parser.Parse("30secs", success_5, GetType(TimeSpan)))
                Assert.IsTrue(success_5)

            End Sub

            <Test()> Public Sub CheckTimeSpanParsing_2()

                Dim c As New TimeSpanConvertor()

                Assert.AreEqual("3 days", _
                    c.ParseStringFromTimespan(New TimeSpan(3, 0, 0, 0), New Boolean))
                Assert.AreEqual("3 hours", _
                    c.ParseStringFromTimespan(New TimeSpan(0, 3, 0, 0), New Boolean))
                Assert.AreEqual("3 mins", _
                    c.ParseStringFromTimespan(New TimeSpan(0, 0, 3, 0), New Boolean))
                Assert.AreEqual("3 secs", _
                    c.ParseStringFromTimespan(New TimeSpan(0, 0, 0, 3), New Boolean))
                Assert.AreEqual("1 day, 1 hour, 1 min, 1 sec", _
                    c.ParseStringFromTimespan(New TimeSpan(1, 1, 1, 1), New Boolean))
                Assert.AreEqual("2 days, 2 hours, 2 mins, 2 secs", _
                    c.ParseStringFromTimespan(New TimeSpan(2, 2, 2, 2), New Boolean))
            End Sub

            <Test()> Public Sub CheckLongParsing()

                Dim c As New LongConvertor()

                Assert.AreEqual("3", _
                    c.ParseStringFromLong(3, New Boolean, GetType(System.String)))

                Assert.AreEqual("30", _
                    c.ParseStringFromLong(30, New Boolean, GetType(System.String)))

                Assert.AreEqual("300", _
                    c.ParseStringFromLong(300, New Boolean, GetType(System.String)))

                Assert.AreEqual("3,000", _
                    c.ParseStringFromLong(3000, New Boolean, GetType(System.String)))

                Assert.AreEqual("-3", _
                    c.ParseStringFromLong(-3, New Boolean, GetType(System.String)))

                Assert.AreEqual("-30", _
                    c.ParseStringFromLong(-30, New Boolean, GetType(System.String)))

                Assert.AreEqual("-300", _
                    c.ParseStringFromLong(-300, New Boolean, GetType(System.String)))

                Assert.AreEqual("-3,000", _
                    c.ParseStringFromLong(-3000, New Boolean, GetType(System.String)))

            End Sub
        End Class

    End Namespace

#End If
