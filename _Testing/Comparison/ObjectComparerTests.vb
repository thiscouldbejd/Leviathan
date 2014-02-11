#If TESTING Then

Imports Leviathan.Comparison
Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

#Region " Sortable Objects "

    Public Class SortableObject1

        Private m_String_1 As String

        Private m_String_2 As String

        <Comparable()> _
        Public Property String_1() As String
            Get
                Return m_String_1
            End Get
            Set(ByVal value As String)
                m_String_1 = value
            End Set
        End Property

        <Comparable()> _
        Public Property String_2() As String
            Get
                Return m_String_2
            End Get
            Set(ByVal value As String)
                m_String_2 = value
            End Set
        End Property

        Public Sub New(ByVal string_1 As String, ByVal string_2 As String)
            Me.String_1 = string_1
            Me.String_2 = string_2
        End Sub
    End Class

    Public Class SortableObject2

        Private m_Property_1 As SortableObject1

        <Comparable()> _
        Public Property Property_1() As SortableObject1
            Get
                Return m_Property_1
            End Get
            Set(ByVal value As SortableObject1)
                m_Property_1 = value
            End Set
        End Property

        Public Sub New(ByVal property_1 As SortableObject1)
            Me.Property_1 = property_1
        End Sub
    End Class

#End Region

    <TestFixture()> _
    Public Class ObjectComparerTests

#Region " General Testing "

        <Test(Description:="String Sort Testing")> _
        Public Sub StringSort_Test1()

            Dim c As New Comparer(ComponentModel.ListSortDirection.Ascending)

            Dim strings As String() = New String() {"bill", "adam", "steve", "alex", "joanna"}

            Array.Sort(strings, c)

            Assert.IsNotNull(strings)
            Assert.AreEqual(5, strings.Length)
            Assert.AreEqual("adam", strings(0))
            Assert.AreEqual("alex", strings(1))
            Assert.AreEqual("bill", strings(2))
            Assert.AreEqual("joanna", strings(3))
            Assert.AreEqual("steve", strings(4))

            c.SortDirection = ComponentModel.ListSortDirection.Descending

            Array.Sort(strings, c)

            Assert.IsNotNull(strings)
            Assert.AreEqual(5, strings.Length)
            Assert.AreEqual("steve", strings(0))
            Assert.AreEqual("joanna", strings(1))
            Assert.AreEqual("bill", strings(2))
            Assert.AreEqual("alex", strings(3))
            Assert.AreEqual("adam", strings(4))

        End Sub

        <Test(Description:="Integer Sort Testing")> _
        Public Sub IntegerSort_Test1()

            Dim c As New Comparer(ComponentModel.ListSortDirection.Ascending)

            Dim numbers As Integer() = New Integer() {4, 6, 1, 5555, 9}

            Array.Sort(numbers, c)

            Assert.IsNotNull(numbers)
            Assert.AreEqual(5, numbers.Length)
            Assert.AreEqual(1, numbers(0))
            Assert.AreEqual(4, numbers(1))
            Assert.AreEqual(6, numbers(2))
            Assert.AreEqual(9, numbers(3))
            Assert.AreEqual(5555, numbers(4))

            c.SortDirection = ComponentModel.ListSortDirection.Descending

            Array.Sort(numbers, c)

            Assert.IsNotNull(numbers)
            Assert.AreEqual(5, numbers.Length)
            Assert.AreEqual(5555, numbers(0))
            Assert.AreEqual(9, numbers(1))
            Assert.AreEqual(6, numbers(2))
            Assert.AreEqual(4, numbers(3))
            Assert.AreEqual(1, numbers(4))

        End Sub

        <Test(Description:="Object Sort Testing 1")> _
        Public Sub ObjectSort_Test1()

            Dim c As New Comparer(ComponentModel.ListSortDirection.Ascending)

            '{"bill", "adam", "steve", "alex", "joanna"}
            Dim objects As SortableObject1() = New SortableObject1() _
            { _
                New SortableObject1("bill", String.Empty), _
                New SortableObject1("adam", String.Empty), _
                New SortableObject1("steve", String.Empty), _
                New SortableObject1("alex", String.Empty), _
                New SortableObject1("joanna", String.Empty) _
            }

            Array.Sort(objects, c)

            Assert.IsNotNull(objects)
            Assert.AreEqual(5, objects.Length)
            Assert.AreEqual("adam", objects(0).String_1)
            Assert.AreEqual("alex", objects(1).String_1)
            Assert.AreEqual("bill", objects(2).String_1)
            Assert.AreEqual("joanna", objects(3).String_1)
            Assert.AreEqual("steve", objects(4).String_1)

            c.SortDirection = ComponentModel.ListSortDirection.Descending

            Array.Sort(objects, c)

            Assert.IsNotNull(objects)
            Assert.AreEqual(5, objects.Length)
            Assert.AreEqual("steve", objects(0).String_1)
            Assert.AreEqual("joanna", objects(1).String_1)
            Assert.AreEqual("bill", objects(2).String_1)
            Assert.AreEqual("alex", objects(3).String_1)
            Assert.AreEqual("adam", objects(4).String_1)

        End Sub

        <Test(Description:="Object Sort Testing 2")> _
        Public Sub ObjectSort_Test2()

            Dim c As New Comparer(ComponentModel.ListSortDirection.Ascending)

            Dim objects_1 As SortableObject1() = New SortableObject1() _
            { _
                New SortableObject1("bill", String.Empty), _
                New SortableObject1("adam", "b"), _
                New SortableObject1("adam", "a"), _
                New SortableObject1("steve", "z"), _
                New SortableObject1("steve", "ee"), _
                New SortableObject1("alex", String.Empty), _
                New SortableObject1("joanna", String.Empty) _
            }

            Array.Sort(objects_1, c)

            Assert.IsNotNull(objects_1)
            Assert.AreEqual(7, objects_1.Length)
            Assert.AreEqual("adam", objects_1(0).String_1)
            Assert.AreEqual("a", objects_1(0).String_2)
            Assert.AreEqual("adam", objects_1(1).String_1)
            Assert.AreEqual("b", objects_1(1).String_2)
            Assert.AreEqual("alex", objects_1(2).String_1)
            Assert.AreEqual("bill", objects_1(3).String_1)
            Assert.AreEqual("joanna", objects_1(4).String_1)
            Assert.AreEqual("steve", objects_1(5).String_1)
            Assert.AreEqual("ee", objects_1(5).String_2)
            Assert.AreEqual("steve", objects_1(6).String_1)
            Assert.AreEqual("z", objects_1(6).String_2)

            c.SortDirection = ComponentModel.ListSortDirection.Descending

            Array.Sort(objects_1, c)

            Assert.IsNotNull(objects_1)
            Assert.AreEqual(7, objects_1.Length)
            Assert.AreEqual("adam", objects_1(6).String_1)
            Assert.AreEqual("a", objects_1(6).String_2)
            Assert.AreEqual("adam", objects_1(5).String_1)
            Assert.AreEqual("b", objects_1(5).String_2)
            Assert.AreEqual("alex", objects_1(4).String_1)
            Assert.AreEqual("bill", objects_1(3).String_1)
            Assert.AreEqual("joanna", objects_1(2).String_1)
            Assert.AreEqual("steve", objects_1(1).String_1)
            Assert.AreEqual("ee", objects_1(1).String_2)
            Assert.AreEqual("steve", objects_1(0).String_1)
            Assert.AreEqual("z", objects_1(0).String_2)

            Dim objects_2 As SortableObject2() = New SortableObject2() _
            { _
                New SortableObject2(New SortableObject1("bill", String.Empty)), _
                New SortableObject2(New SortableObject1("adam", "b")), _
                New SortableObject2(New SortableObject1("adam", "a")), _
                New SortableObject2(New SortableObject1("steve", "z")), _
                New SortableObject2(New SortableObject1("steve", "ee")), _
                New SortableObject2(New SortableObject1("alex", String.Empty)), _
                New SortableObject2(New SortableObject1("joanna", String.Empty)) _
            }

            c.SortDirection = ComponentModel.ListSortDirection.Ascending
            Array.Sort(objects_2, c)

            Assert.IsNotNull(objects_2)
            Assert.AreEqual(7, objects_2.Length)
            Assert.AreEqual("adam", objects_2(0).Property_1.String_1)
            Assert.AreEqual("a", objects_2(0).Property_1.String_2)
            Assert.AreEqual("adam", objects_2(1).Property_1.String_1)
            Assert.AreEqual("b", objects_2(1).Property_1.String_2)
            Assert.AreEqual("alex", objects_2(2).Property_1.String_1)
            Assert.AreEqual("bill", objects_2(3).Property_1.String_1)
            Assert.AreEqual("joanna", objects_2(4).Property_1.String_1)
            Assert.AreEqual("steve", objects_2(5).Property_1.String_1)
            Assert.AreEqual("ee", objects_2(5).Property_1.String_2)
            Assert.AreEqual("steve", objects_2(6).Property_1.String_1)
            Assert.AreEqual("z", objects_2(6).Property_1.String_2)

            c.SortDirection = ComponentModel.ListSortDirection.Descending

            Array.Sort(objects_2, c)

            Assert.IsNotNull(objects_2)
            Assert.AreEqual(7, objects_2.Length)
            Assert.AreEqual("adam", objects_2(6).Property_1.String_1)
            Assert.AreEqual("a", objects_2(6).Property_1.String_2)
            Assert.AreEqual("adam", objects_2(5).Property_1.String_1)
            Assert.AreEqual("b", objects_2(5).Property_1.String_2)
            Assert.AreEqual("alex", objects_2(4).Property_1.String_1)
            Assert.AreEqual("bill", objects_2(3).Property_1.String_1)
            Assert.AreEqual("joanna", objects_2(2).Property_1.String_1)
            Assert.AreEqual("steve", objects_2(1).Property_1.String_1)
            Assert.AreEqual("ee", objects_2(1).Property_1.String_2)
            Assert.AreEqual("steve", objects_2(0).Property_1.String_1)
            Assert.AreEqual("z", objects_2(0).Property_1.String_2)

        End Sub
#End Region

    End Class

End Namespace

#End If
