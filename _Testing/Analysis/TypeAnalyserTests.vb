#If TESTING Then

Imports System.Configuration
Imports NUnit.Framework

Namespace Testing

    <TestFixture()> _
    Public Class TypeAnalyserTests

#Region " Public Properties "

        Public ReadOnly Property Analyser_1() As TypeAnalyser
            Get
                Return TypeAnalyser.GetInstance(GetType(TestAnalysedObject_1))
            End Get
        End Property

        Public ReadOnly Property Analyser_2() As TypeAnalyser
            Get
                Return TypeAnalyser.GetInstance(GetType(TestAnalysedObject_2))
            End Get
        End Property

        Public ReadOnly Property Analyser_3() As TypeAnalyser
            Get
                Return TypeAnalyser.GetInstance(GetType(TestAnalysedObject_3))
            End Get
        End Property

        Public ReadOnly Property Analyser_4() As TypeAnalyser
            Get
                Return TypeAnalyser.GetInstance(GetType(TestAnalysedObject_4))
            End Get
        End Property

        Public ReadOnly Property Analyser_5() As TypeAnalyser
            Get
                Return TypeAnalyser.GetInstance(GetType(TestAnalysedObject_5))
            End Get
        End Property

        Public ReadOnly Property Analyser_Type() As TypeAnalyser
            Get
                Return TypeAnalyser.GetInstance(GetType(System.Type))
            End Get
        End Property

        Public ReadOnly Property Analyser_Integer() As TypeAnalyser
            Get
                Return TypeAnalyser.GetInstance(GetType(System.Int32))
            End Get
        End Property

        Public ReadOnly Property Analyser_String() As TypeAnalyser
            Get
                Return TypeAnalyser.GetInstance(GetType(System.String))
            End Get
        End Property

        Public ReadOnly Property Analyser_Integer_Array() As TypeAnalyser
            Get
                Return TypeAnalyser.GetInstance(GetType(System.Int32).MakeArrayType)
            End Get
        End Property

        Public ReadOnly Property Analyser_String_Array() As TypeAnalyser
            Get
                Return TypeAnalyser.GetInstance(GetType(System.String).MakeArrayType)
            End Get
        End Property

#End Region

#Region " Singleton Tests "

        <Test()> Public Sub CheckSingleton()

            Dim analyser_1 As TypeAnalyser = _
                TypeAnalyser.GetInstance(GetType(TestAnalysedObject_1))

            Assert.AreEqual(GetType(TestAnalysedObject_1), analyser_1.Type)

            Dim analyser_2 As TypeAnalyser = _
                TypeAnalyser.GetInstance(GetType(TestAnalysedObject_1))

            Assert.AreEqual(GetType(TestAnalysedObject_1), analyser_2.Type)

            Assert.AreSame(analyser_1, analyser_2)
        End Sub

#End Region

#Region " Simple Tests "

        <Test(Description:="Analyser Type Testing")> _
                Public Sub CheckTypes()

            Assert.AreEqual(GetType(System.Type), Analyser_Type.Type)

            Assert.AreEqual(GetType(System.Int32), Analyser_Integer.Type)
            Assert.AreEqual(GetType(System.String), Analyser_String.Type)

            Assert.AreEqual(GetType(System.Int32).MakeArrayType, Analyser_Integer_Array.Type)
            Assert.AreEqual(GetType(System.String).MakeArrayType, Analyser_String_Array.Type)

            Assert.AreEqual(GetType(TestAnalysedObject_1), Analyser_1.Type)
            Assert.AreEqual(GetType(TestAnalysedObject_2), Analyser_2.Type)
            Assert.AreEqual(GetType(TestAnalysedObject_3), Analyser_3.Type)
            Assert.AreEqual(GetType(TestAnalysedObject_4), Analyser_4.Type)
            Assert.AreEqual(GetType(TestAnalysedObject_5), Analyser_5.Type)

        End Sub

        <Test(Description:="Analyser Name Testing")> _
        Public Sub CheckNames()

            Assert.AreEqual("Type", Analyser_Type.Name)

            Assert.AreEqual("Int32", Analyser_Integer.Name)
            Assert.AreEqual("String", Analyser_String.Name)

            Assert.AreEqual("Int32[]", Analyser_Integer_Array.Name)
            Assert.AreEqual("String[]", Analyser_String_Array.Name)

            Assert.AreEqual("TestAnalysedObject_1", Analyser_1.Name)
            Assert.AreEqual("TestAnalysedObject_2", Analyser_2.Name)
            Assert.AreEqual("TestAnalysedObject_3", Analyser_3.Name)
            Assert.AreEqual("TestAnalysedObject_4", Analyser_4.Name)
            Assert.AreEqual("TestAnalysedObject_5", Analyser_5.Name)

            Assert.AreEqual("System.Type", Analyser_Type.ToString)

            Assert.AreEqual("System.Int32", Analyser_Integer.ToString)
            Assert.AreEqual("System.String", Analyser_String.ToString)

            Assert.AreEqual("System.Int32[]", Analyser_Integer_Array.ToString)
            Assert.AreEqual("System.String[]", Analyser_String_Array.ToString)

            Assert.AreEqual("Leviathan.TestAnalysedObject_1", Analyser_1.ToString)
            Assert.AreEqual("Leviathan.TestAnalysedObject_2", Analyser_2.ToString)
            Assert.AreEqual("Leviathan.TestAnalysedObject_3", Analyser_3.ToString)
            Assert.AreEqual("Leviathan.TestAnalysedObject_4", Analyser_4.ToString)
            Assert.AreEqual("Leviathan.TestAnalysedObject_5", Analyser_5.ToString)

        End Sub

        <Test(Description:="Analyser Simple Type Testing")> _
        Public Sub CheckSimpleTypes()

            Assert.IsTrue(Analyser_Type.IsSimple)

            Assert.IsTrue(Analyser_Integer.IsSimple)
            Assert.IsTrue(Analyser_String.IsSimple)

            Assert.IsFalse(Analyser_Integer_Array.IsSimple)
            Assert.IsFalse(Analyser_String_Array.IsSimple)

            Assert.IsTrue(Analyser_Integer_Array.IsSimpleArray)
            Assert.IsTrue(Analyser_String_Array.IsSimpleArray)

            Assert.IsFalse(Analyser_1.IsSimple)
            Assert.IsFalse(Analyser_2.IsSimple)
            Assert.IsFalse(Analyser_3.IsSimple)
            Assert.IsFalse(Analyser_4.IsSimple)
            Assert.IsFalse(Analyser_5.IsSimple)

        End Sub

        <Test(Description:="Analyser Complex Type Testing")> _
        Public Sub CheckComplexTypes()

            Assert.IsFalse(Analyser_Type.IsComplex)

            Assert.IsFalse(Analyser_Integer.IsComplex)
            Assert.IsFalse(Analyser_String.IsComplex)

            Assert.IsTrue(Analyser_Integer_Array.IsComplex)
            Assert.IsTrue(Analyser_String_Array.IsComplex)

            Assert.IsFalse(Analyser_Integer_Array.IsComplexArray)
            Assert.IsFalse(Analyser_String_Array.IsComplexArray)

            Assert.IsTrue(Analyser_1.IsComplex)
            Assert.IsTrue(Analyser_2.IsComplex)
            Assert.IsTrue(Analyser_3.IsComplex)
            Assert.IsTrue(Analyser_4.IsComplex)
            Assert.IsTrue(Analyser_5.IsComplex)

        End Sub

        <Test(Description:="Analyser Array Type Testing")> _
        Public Sub CheckArrayTypes()

            Assert.IsFalse(Analyser_Type.IsArray)

            Assert.IsFalse(Analyser_Integer.IsArray)
            Assert.IsFalse(Analyser_String.IsArray)

            Assert.IsTrue(Analyser_Integer_Array.IsArray)
            Assert.IsTrue(Analyser_String_Array.IsArray)

            Assert.IsFalse(Analyser_1.IsArray)
            Assert.IsFalse(Analyser_2.IsArray)
            Assert.IsFalse(Analyser_3.IsArray)
            Assert.IsFalse(Analyser_4.IsArray)
            Assert.IsFalse(Analyser_5.IsArray)

        End Sub

        <Test(Description:="Analyser Collection Type Testing")> _
        Public Sub CheckCollectionTypes()

            Assert.IsFalse(Analyser_Type.IsICollection)

            Assert.IsFalse(Analyser_Integer.IsICollection)
            Assert.IsFalse(Analyser_String.IsICollection)

            Assert.IsTrue(Analyser_Integer_Array.IsICollection)
            Assert.IsTrue(Analyser_String_Array.IsICollection)

            Assert.IsFalse(Analyser_1.IsICollection)
            Assert.IsFalse(Analyser_2.IsICollection)
            Assert.IsFalse(Analyser_3.IsICollection)
            Assert.IsFalse(Analyser_4.IsICollection)
            Assert.IsFalse(Analyser_5.IsICollection)

        End Sub

        <Test(Description:="Analyser Array Type Testing")> _
        Public Sub CheckTypeTypes()

            Assert.IsTrue(Analyser_Type.IsType)

            Assert.IsFalse(Analyser_Integer.IsType)
            Assert.IsFalse(Analyser_String.IsType)

            Assert.IsFalse(Analyser_Integer_Array.IsType)
            Assert.IsFalse(Analyser_String_Array.IsType)

            Assert.IsFalse(Analyser_1.IsType)
            Assert.IsFalse(Analyser_2.IsType)
            Assert.IsFalse(Analyser_3.IsType)
            Assert.IsFalse(Analyser_4.IsType)
            Assert.IsFalse(Analyser_5.IsType)

        End Sub

        <Test(Description:="Analyser Element Type Testing")> _
        Public Sub CheckElementTypes()

            Assert.AreEqual(GetType(Type), Analyser_Type.ElementType)

            Assert.AreEqual(GetType(Integer), Analyser_Integer.ElementType)
            Assert.AreEqual(GetType(String), Analyser_String.ElementType)

            Assert.AreEqual(GetType(Integer), Analyser_Integer_Array.ElementType)
            Assert.AreEqual(GetType(String), Analyser_String_Array.ElementType)


            Assert.AreEqual(GetType(TestAnalysedObject_1), Analyser_1.ElementType)
            Assert.AreEqual(GetType(TestAnalysedObject_2), Analyser_2.ElementType)
            Assert.AreEqual(GetType(TestAnalysedObject_3), Analyser_3.ElementType)
            Assert.AreEqual(GetType(TestAnalysedObject_4), Analyser_4.ElementType)
            Assert.AreEqual(GetType(TestAnalysedObject_5), Analyser_5.ElementType)

        End Sub

        <Test(Description:="Default Constructor Testing")> _
        Public Sub CheckSimpleConstructor()

            Assert.IsFalse(Analyser_Type.HasDefaultConstructor)

            Assert.IsFalse(Analyser_Integer.HasDefaultConstructor)
            Assert.IsFalse(Analyser_String.HasDefaultConstructor)

            Assert.IsFalse(Analyser_Integer_Array.HasDefaultConstructor)
            Assert.IsFalse(Analyser_String_Array.HasDefaultConstructor)

            Assert.IsTrue(Analyser_1.HasDefaultConstructor)
            Assert.IsTrue(Analyser_2.HasDefaultConstructor)
            Assert.IsTrue(Analyser_3.HasDefaultConstructor)
            Assert.IsTrue(Analyser_4.HasDefaultConstructor)
            Assert.IsFalse(Analyser_5.HasDefaultConstructor)

        End Sub

#End Region

#Region " Execute Query Tests "

        <Test(Description:="Tests Member Querying with Visibility and Location.")> _
        Public Sub CheckSimpleQuery_1()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All) _
                    )

            CheckMemberNames(members, "m_Field1", "m_Field2", "Field3", "Property1", _
                            "Property2", "Property3", "Property4", "Property5")

        End Sub

        <Test(Description:="Tests Member Querying with Visibility and Location.")> _
        Public Sub CheckSimpleQuery_2()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.Public) _
                        .SetLocation(MemberLocation.All) _
                    )

            CheckMemberNames(members, "Field3", "Property1", _
                            "Property2", "Property3")

        End Sub

        <Test(Description:="Tests Member Querying with Visibility and Location.")> _
        Public Sub CheckSimpleQuery_3()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.NonPublic) _
                        .SetLocation(MemberLocation.All) _
                    )

            CheckMemberNames(members, "m_Field1", "m_Field2", _
                            "Property4", "Property5")

        End Sub

        <Test(Description:="Tests Member Querying with Visibility and Location.")> _
        Public Sub CheckSimpleQuery_4()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.Protected) _
                        .SetLocation(MemberLocation.All) _
                    )

            CheckMemberNames(members, "Property5")

        End Sub

        <Test(Description:="Tests Constructor Querying with Visibility and Location.")> _
        Public Sub CheckConstructorQuery_1()

            Dim methods As MemberAnalyser() = _
                TypeAnalyser.GetInstance(GetType(TestAnalysedObject_1)) _
                    .ExecuteQuery( _
                        New AnalyserQuery() _
                            .SetReturnType(AnalyserType.ConstructorAnalyser) _
                            .SetVisibility(MemberVisibility.All) _
                            .SetLocation(MemberLocation.All) _
                            .SetDeclaredBelowType(GetType(Object)) _
                        )

            CheckMemberNames(methods, ".ctor", ".ctor", _
                            ".ctor", ".ctor")

        End Sub

        <Test(Description:="Tests Constructor Querying with Visibility and Location.")> _
        Public Sub CheckConstructorQuery_2()

            Dim methods As MemberAnalyser() = _
                TypeAnalyser.GetInstance(GetType(TestAnalysedObject_1)) _
                            .ExecuteQuery( _
                                New AnalyserQuery() _
                                .SetReturnType(AnalyserType.ConstructorAnalyser) _
                                .SetArgumentCount(1) _
                                .SetLocation(MemberLocation.All) _
                                .SetVisibility(MemberVisibility.All) _
                            )

            CheckMemberNames(methods, ".ctor")

        End Sub

        <Test(Description:="Tests Constructor Querying with Visibility and Location.")> _
        Public Sub CheckConstructorQuery_3()

            Dim methods As MemberAnalyser() = _
                TypeAnalyser.GetInstance(GetType(TestAnalysedObject_1)) _
                    .ExecuteQuery( _
                        New AnalyserQuery() _
                            .SetReturnType(AnalyserType.ConstructorAnalyser) _
                            .SetArgumentCount(2) _
                            .SetLocation(MemberLocation.All) _
                            .SetVisibility(MemberVisibility.All) _
                        )

            CheckMemberNames(methods, ".ctor", ".ctor")

        End Sub

        <Test(Description:="Tests Constructor Querying with Visibility and Location.")> _
        Public Sub CheckMethodQuery_1()

            Dim methods As MemberAnalyser() = _
                TypeAnalyser.GetInstance(GetType(TestAnalysedObject_1)) _
                    .ExecuteQuery( _
                        New AnalyserQuery() _
                            .SetReturnType(AnalyserType.MethodAnalyser) _
                            .SetArgumentCount(1) _
                            .SetVisibility(MemberVisibility.All) _
                            .SetLocation(MemberLocation.All) _
                        )

            CheckMemberNames(methods, "set_Property1", "set_Property3", "set_Property4", _
                                "set_Property5", "Equals")

        End Sub

        <Test(Description:="Tests Constructor Querying with Visibility and Location.")> _
        Public Sub CheckMethodQuery_2()

            Dim methods As MemberAnalyser() = _
                TypeAnalyser.GetInstance(GetType(TestAnalysedObject_1)) _
                    .ExecuteQuery( _
                        New AnalyserQuery() _
                            .SetReturnType(AnalyserType.MethodAnalyser) _
                            .SetArgumentCount(2) _
                            .SetVisibility(MemberVisibility.All) _
                            .SetLocation(MemberLocation.All) _
                        )

            CheckMemberNames(methods, "Method1", "Equals", _
                                "ReferenceEquals", "GetFieldInfo")

        End Sub

        <Test(Description:="Tests Member Querying for Fields")> _
        Public Sub CheckFieldQuery_1()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All) _
                        .SetIsVariable() _
                    )

            CheckMemberNames(members, "m_Field1", "m_Field2", "Field3")

            CheckMemberReturnTypes(members, GetType(Object), GetType(String), GetType(Object))

        End Sub

        <Test(Description:="Tests Read-Only Property Querying")> _
        Public Sub CheckPropertyQuery_1()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All) _
                        .SetAccessibility(MemberAccessibility.ReadOnly) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property2")

        End Sub

        <Test(Description:="Tests Write-Only Property Querying")> _
        Public Sub CheckPropertyQuery_2()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetLocation(MemberLocation.All) _
                        .SetAccessibility(MemberAccessibility.WriteOnly) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property3")

        End Sub

        <Test(Description:="Tests Property Name Querying")> _
        Public Sub CheckPropertyQuery_3()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetNames("Property4", "Property5") _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property4", "Property5")

        End Sub

        <Test(Description:="Tests Property Private Querying")> _
        Public Sub CheckPropertyQuery_4()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.NonPublic) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property4", "Property5")

        End Sub

        <Test(Description:="Tests Property Public Querying")> _
        Public Sub CheckPropertyQuery_5()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.Public) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property1", "Property2", "Property3")

        End Sub

        <Test(Description:="Tests Property Read/Write Querying")> _
        Public Sub CheckPropertyQuery_6()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetAccessibility(MemberAccessibility.ReadWrite) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property1", "Property4", "Property5")

        End Sub

        <Test(Description:="Tests Property Read Only Querying")> _
        Public Sub CheckPropertyQuery_7()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetAccessibility(MemberAccessibility.ReadOnly) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property2")

        End Sub

        <Test(Description:="Tests Property Write Only Querying")> _
        Public Sub CheckPropertyQuery_8()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetAccessibility(MemberAccessibility.WriteOnly) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property3")

        End Sub

        <Test(Description:="Tests Property Readable Querying")> _
        Public Sub CheckPropertyQuery_9()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetAccessibility(MemberAccessibility.Readable) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property1", "Property2", "Property4", "Property5")

        End Sub

        <Test(Description:="Tests Property Readable Querying")> _
        Public Sub CheckPropertyQuery_10()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetAccessibility(MemberAccessibility.Writable) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property1", "Property3", "Property4", "Property5")

        End Sub

        <Test(Description:="Tests Property Return Querying")> _
        Public Sub CheckPropertyQuery_11()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetReturnTypeIsOrInheritedFromType(GetType(String)) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property4")

        End Sub

        <Test(Description:="Tests Property Name Querying")> _
        Public Sub CheckPropertyQuery_12()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetName("Property5") _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property5")

        End Sub

        <Test(Description:="Tests Property Name Querying")> _
        Public Sub CheckPropertyQuery_13()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetArgumentCount(0) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property1", "Property2", "Property3", "Property4", "Property5")

        End Sub

        <Test(Description:="Tests Member Return")> _
        Public Sub CheckMember_1()

            Dim member As MemberAnalyser = _
                Analyser_2.GetSingleMember(GetType(TestAnalysedObject_1))

            Assert.IsNotNull(member)
            Assert.AreEqual("Property1", member.FullName)

        End Sub

        <Test(Description:="Tests Member Return")> _
        Public Sub CheckMember_2()

            Dim member As MemberAnalyser = _
                Analyser_3.GetSingleMember(GetType(TestAnalysedObject_1))

            Assert.IsNull(member)

        End Sub

        <Test(Description:="Tests Member Return")> _
        Public Sub CheckMember_3()

            Dim member As MemberAnalyser = _
                Analyser_3.GetSingleMember(GetType(TestAnalysedObject_3))

            Assert.IsNotNull(member)
            Assert.AreEqual("Property4", member.FullName)

        End Sub

        <Test(Description:="Tests Attribute Querying")> _
        Public Sub CheckAttributeQuery_1()

            Dim member As AttributeAnalyser = _
                Analyser_1.GetAttribute(GetType(TestAttribute_1))

            Assert.IsNotNull(member)
            Assert.AreEqual("TestAnalysedObjectAttribute", _
                CType(member.Attribute, TestAttribute_1).Name)

        End Sub

        <Test(Description:="Tests Attribute Querying")> _
        Public Sub CheckAttributeQuery_2()

            Dim methods As MemberAnalyser() = _
                Analyser_4.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MethodAnalyser) _
                        .SetPresentAttribute(GetType(TestAttribute_1)) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                    )

            CheckMemberNames(methods, "Method2")

        End Sub

        <Test(Description:="Tests Attributed Property Querying")> _
        Public Sub CheckAttributeQuery_3()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetPresentAttribute(GetType(TestAttribute_1)) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                    )

            CheckMemberNames(members, "Property1", "Property4")

        End Sub

        <Test(Description:="Tests Attributed Property Querying")> _
        Public Sub CheckAttributeQuery_4()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetPresentAttribute(GetType(TestAttribute_2)) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property2", "Property4")

        End Sub

        <Test(Description:="Tests Attributed Property Querying")> _
        Public Sub CheckAttributeQuery_5()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetNotPresentAttribute(GetType(TestAttribute_1)) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property2", "Property3", "Property5")

        End Sub

        <Test(Description:="Tests Attributed Property Querying")> _
        Public Sub CheckAttributeQuery_6()

            Dim members As MemberAnalyser() = _
                Analyser_1.ExecuteQuery( _
                    New AnalyserQuery() _
                        .SetReturnType(AnalyserType.MemberAnalyser) _
                        .SetNotPresentAttribute(GetType(TestAttribute_2)) _
                        .SetLocation(MemberLocation.All) _
                        .SetVisibility(MemberVisibility.All) _
                        .SetNotIsVariable() _
                    )

            CheckMemberNames(members, "Property1", "Property3", "Property5")

        End Sub

#End Region

#Region " Private Methods "

        Private Sub CheckMemberNames( _
            ByVal members As MemberAnalyser(), _
            ByVal ParamArray expectedNames As String() _
        )

            Assert.IsNotNull(members)
            Assert.AreEqual(expectedNames.Length, members.Length)

            For i As Integer = 0 To expectedNames.Length - 1
                Assert.AreEqual(expectedNames(i), members(i).Name)
            Next

        End Sub

        Private Sub CheckMemberReturnTypes( _
            ByVal members As MemberAnalyser(), _
            ByVal ParamArray expectedTypes As Type() _
        )

            Assert.IsNotNull(members)
            Assert.AreEqual(expectedTypes.Length, members.Length)

            For i As Integer = 0 To expectedTypes.Length - 1
                Assert.AreEqual(expectedTypes(i), members(i).ReturnType)
            Next

        End Sub

#End Region

    End Class

End Namespace

#End If