#If TESTING Then

Imports System.Configuration
Imports NUnit.Framework

Namespace Testing
	
        <TestFixture()> _
        Public Class ArgumentsProcessorTest

            Private m_ArgProcessor

            Public ReadOnly Property ArgProcessor() As Commands.CommandArgumentsParser
                Get
                    If m_ArgProcessor Is Nothing Then
                        m_ArgProcessor = New Commands.CommandArgumentsParser( _
                            New Char() {QUESTION_MARK}, _
                            New Char() {QUOTE_SINGLE, QUOTE_DOUBLE})
                    End If
                    Return m_ArgProcessor
                End Get
            End Property

            <Test()> Public Sub CheckInput_1()
                GenericLineParseTest( _
                "a ""?b c ""d=?e f g=h?""?"" =:i;j.k;l.m;n.o", _
                "a", "?b c ""d=?e f g=h?""?", "=:i;j.k;l.m;n.o")
            End Sub

            <Test()> Public Sub CheckInput_2()
                GenericLineParseTest( _
                "a b c 'd e'", _
                "a", "b", "c", "d e")
            End Sub

            <Test()> Public Sub CheckInput_3()
                GenericLineParseTest( _
                "a b c 'd ""?e f 'g'?"" h'", _
                "a", "b", "c", "d ""?e f 'g'?"" h")
            End Sub

            <Test()> Public Sub CheckInput_4()
                GenericLineParseTest( _
                "a b ?c d e.f=G h>01/01/07 i=?j k l>01/06/07 e.f=1M /d? /d? /r:s /c:c;s /v:i /r:a $:s /d", _
                "a", "b", "?c d e.f=G h>01/01/07 i=?j k l>01/06/07 e.f=1M /d? /d?", "/r:s", "/c:c;s", "/v:i", "/r:a", "$:s", "/d")
            End Sub

            <Test()> Public Sub CheckInput_5()
                GenericLineParseTest("a b ?c d?", "a", "b", "?c d?")
            End Sub

            <Test()> Public Sub CheckInput_6()
                GenericLineParseTest("a b 'c d'", "a", "b", "c d")
            End Sub

            <Test()> Public Sub CheckInput_7()
                GenericLineParseTest("a b ""c d""", "a", "b", "c d")
            End Sub

            <Test()> Public Sub CheckInput_8()
                GenericLineParseTest("a b ?c ?e f? d?", "a", "b", "?c ?e f? d?")
            End Sub

            <Test()> Public Sub CheckInput_9()
                GenericLineParseTest("a b ?c 'e f' d?", "a", "b", "?c 'e f' d?")
            End Sub

            <Test()> Public Sub CheckInput_10()
                GenericLineParseTest("a b ?c '?e f?' d?", "a", "b", "?c '?e f?' d?")
            End Sub

            <Test()> Public Sub CheckInput_11()
                GenericLineParseTest("a b ?c ""?e f?"" d?", "a", "b", "?c ""?e f?"" d?")
            End Sub

            <Test()> Public Sub CheckInput_12()
                GenericLineParseTest("a b ? ?c d? e?", "a", "b", "? ?c d? e?")
            End Sub

            <Test()> Public Sub CheckInput_13()
                GenericLineParseTest("a ?b c 'd=?e f g h?'? =:i", _
                "a", "?b c 'd=?e f g h?'?", "=:i")
            End Sub

            <Test()> Public Sub CheckInput_14()
                GenericLineParseTest("a ?b c 'd.e=?f g h i?' 'j<24/06/2007 19:54:56'? =:l", _
                    "a", "?b c 'd.e=?f g h i?' 'j<24/06/2007 19:54:56'?", "=:l")
            End Sub

            <Test()> Public Sub CheckInput_15()
                GenericLineParseTest("a ?b c 'd.e=?f g h i?' 'j<24/06/2007 19:54:56' 'k>24/06/2007 19:54:56'? =:l", _
                    "a", "?b c 'd.e=?f g h i?' 'j<24/06/2007 19:54:56' 'k>24/06/2007 19:54:56'?", "=:l")
            End Sub

            <Test()> Public Sub CheckInput_16()
                GenericLineParseTest("a ?b 'd=?f g?'?", _
                    "a", "?b 'd=?f g?'?")
            End Sub

            <Test()> Public Sub CheckInput_17()
                GenericLineParseTest("a ?b d=?f g??", _
                    "a", "?b d=?f g??")
            End Sub

            <Test()> Public Sub CheckInput_18()
                GenericLineParseTest("b d=?f g?", _
                    "b", "d=?f g?")
            End Sub

            <Test()> Public Sub CheckInput_19()
                GenericLineParseTest("a ?b c d=?e f g h??", _
                    "a", "?b c d=?e f g h??")
            End Sub

            <Test()> Public Sub CheckInput_20()
                GenericLineParseTest("b c d=?e f g h?", _
                    "b", "c", "d=?e f g h?")
            End Sub

            <Test()> Public Sub CheckInput_21()
                GenericLineParseTest("a ?b ""c"" ?", _
                    "a", "?b ""c"" ?")
            End Sub

            <Test()> Public Sub CheckInput_22()
                GenericLineParseTest("a ?b ""c""?", _
                    "a", "?b ""c""?")
            End Sub

            <Test()> Public Sub CheckInput_23()
                GenericLineParseTest("a b?c", _
                    "a", "b?c")
            End Sub

            <Test()> Public Sub CheckInput_24()
                GenericLineParseTest("a b?c d", _
                    "a", "b?c", "d")
            End Sub

			<Test()> Public Sub CheckInput_25()
                GenericLineParseTest("b?c d", _
                    "b?c", "d")
            End Sub
            
            <Test()> Public Sub CheckCommandLineParse_1()
                CommandLineParseTest("a b c", "a", "b", "c")
            End Sub

            <Test()> Public Sub CheckCommandLineParse_2()
                CommandLineParseTest("a ""b c""", "a", "b c")
            End Sub

            <Test()> Public Sub CheckCommandLineParse_3()
                CommandLineParseTest("a ""b c"" d e", "a", "b c", "d", "e")
            End Sub

            <Test()> Public Sub CheckArgumentStart()
                Dim c_Space As Char = Char.Parse(" ")
                Dim c_sQuote As Char = Char.Parse("'")
                Dim c_dQuote As Char = Char.Parse("""")
                Dim c_Question As Char = Char.Parse("?")
                Dim c_A As Char = Char.Parse("a")
                Dim testChars As Char()

                '|'a|
                testChars = New Char() {c_sQuote, c_A}
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_sQuote, 0, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_A, 1, testChars))

                '|?a|
                testChars = New Char() {c_Question, c_A}
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_Question, 0, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_A, 1, testChars))

                '|'?a|
                testChars = New Char() {c_sQuote, c_Question, c_A}
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_sQuote, 0, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_Question, 1, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_A, 2, testChars))

                ' Test |??a|
                testChars = New Char() {c_Question, c_Question, c_A}
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_Question, 0, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_Question, 1, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_A, 2, testChars))

                '|'??a|
                testChars = New Char() {c_sQuote, c_Question, c_Question, c_A}
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_sQuote, 0, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_Question, 1, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_Question, 2, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_A, 3, testChars))

                '|a?' |
                testChars = New Char() {c_A, c_Question, c_sQuote, c_Space}
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_A, 0, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_Question, 1, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_sQuote, 2, testChars))

                '|? ?a|
                testChars = New Char() {c_Question, c_Space, c_Question, c_A}
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_Question, 0, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_Question, 2, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_A, 3, testChars))

                '|a?a|
                testChars = New Char() {c_A, c_Question, c_A}
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_A, 0, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentStart(c_Question, 1, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentStart(c_A, 2, testChars))
            End Sub

            <Test()> Public Sub CheckArgumentEnd()
                Dim c_Space As Char = Char.Parse(" ")
                Dim c_sQuote As Char = Char.Parse("'")
                Dim c_dQuote As Char = Char.Parse("""")
                Dim c_Question As Char = Char.Parse("?")
                Dim c_A As Char = Char.Parse("a")
                Dim testChars As Char()

                '|a'|
                testChars = New Char() {c_A, c_sQuote}
                Assert.IsFalse(ArgProcessor.IsArgumentEnd(c_A, 0, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentEnd(c_sQuote, 1, testChars))

                '|a?'|
                testChars = New Char() {c_A, c_Question, c_sQuote}
                Assert.IsFalse(ArgProcessor.IsArgumentEnd(c_A, 0, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentEnd(c_Question, 1, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentEnd(c_sQuote, 2, testChars))

                '|a? a?|
                testChars = New Char() {c_A, c_Question, c_Space, c_A, c_Question}
                Assert.IsFalse(ArgProcessor.IsArgumentEnd(c_A, 0, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentEnd(c_Question, 1, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentEnd(c_Space, 2, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentEnd(c_A, 3, testChars))
                Assert.IsTrue(ArgProcessor.IsArgumentEnd(c_Question, 4, testChars))

                '|a??a|
                testChars = New Char() {c_A, c_Question, c_Question, c_A}
                Assert.IsTrue(ArgProcessor.IsArgumentEnd(c_Question, 1, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentEnd(c_Question, 2, testChars))

                '|a? ?a|
                testChars = New Char() {c_A, c_Question, c_Space, c_Question, c_A}
                Assert.IsTrue(ArgProcessor.IsArgumentEnd(c_Question, 1, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentEnd(c_Question, 3, testChars))

                '|? ?a|
                testChars = New Char() {c_Question, c_Space, c_Question, c_A}
                Assert.IsFalse(ArgProcessor.IsArgumentEnd(c_Question, 0, testChars))
                Assert.IsFalse(ArgProcessor.IsArgumentEnd(c_Question, 2, testChars))
            End Sub

            Public Sub GenericArgParseTest( _
                ByVal testArgs As String(), _
                ByVal ParamArray parsedArguments As String() _
            )

                Dim parsedArgs As String() = ArgProcessor.ParseArray(testArgs)

                Assert.IsNotNull(parsedArgs)
                Assert.AreEqual(parsedArguments.Length, parsedArgs.Length)

                For i As Integer = 0 To parsedArguments.Length - 1
                    Assert.AreEqual(parsedArguments(i), parsedArgs(i))
                Next

            End Sub

            Public Sub GenericLineParseTest( _
                ByVal testLine As String, _
                ByVal ParamArray parsedArguments As String() _
            )

                Dim parsedArgs As String() = ArgProcessor.ParseLine(testLine)

                Assert.IsNotNull(parsedArgs)
                Assert.AreEqual(parsedArguments.Length, parsedArgs.Length)

                For i As Integer = 0 To parsedArguments.Length - 1
                    Assert.AreEqual(parsedArguments(i), parsedArgs(i))
                Next

            End Sub

            Public Sub CommandLineParseTest( _
                ByVal testLine As String, _
                ByVal ParamArray parsedArguments As String() _
            )

                Dim parsedArgs As String() = Commands.CommandArgumentsParser.CommandLineParse(testLine)

                Assert.IsNotNull(parsedArgs)
                Assert.AreEqual(parsedArguments.Length, parsedArgs.Length)

                For i As Integer = 0 To parsedArguments.Length - 1
                    Assert.AreEqual(parsedArguments(i), parsedArgs(i))
                Next

            End Sub

        End Class

    End Namespace

#End If