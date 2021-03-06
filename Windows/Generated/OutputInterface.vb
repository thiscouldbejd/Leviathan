''' <summary></summary>
''' <autogenerated>Generated from a T4 template. Modifications will be lost, if applicable use a partial class instead.</autogenerated>
''' <generator-date>10/02/2014 15:39:42</generator-date>
''' <generator-functions>1</generator-functions>
''' <generator-source>Leviathan\Windows\Generated\OutputInterface.tt</generator-source>
''' <generator-template>Text-Templates\Classes\VB_Object.tt</generator-template>
''' <generator-version>1</generator-version>
Partial Public Class OutputInterface
	Inherits System.Windows.Window

	#Region " Public Constructors "

		''' <summary>Default Constructor</summary>
		Public Sub New()

			MyBase.New()

			m_OPACITY_ACTIVE = 0.95
			m_OPACITY_INACTIVE = 0.6
			m_OPACITY_FAST = 100
			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (1 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String _
		)

			MyBase.New()

			Command = _Command

			m_OPACITY_ACTIVE = 0.95
			m_OPACITY_INACTIVE = 0.6
			m_OPACITY_FAST = 100
			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (2 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32 _
		)

			MyBase.New()

			Command = _Command
			Id = _Id

			m_OPACITY_ACTIVE = 0.95
			m_OPACITY_INACTIVE = 0.6
			m_OPACITY_FAST = 100
			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (3 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed

			m_OPACITY_ACTIVE = 0.95
			m_OPACITY_INACTIVE = 0.6
			m_OPACITY_FAST = 100
			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (4 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress

			m_OPACITY_ACTIVE = 0.95
			m_OPACITY_INACTIVE = 0.6
			m_OPACITY_FAST = 100
			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (5 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput

			m_OPACITY_ACTIVE = 0.95
			m_OPACITY_INACTIVE = 0.6
			m_OPACITY_FAST = 100
			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (6 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String, _
			ByVal _ReturnedInput As System.Boolean _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput
			ReturnedInput = _ReturnedInput

			m_OPACITY_ACTIVE = 0.95
			m_OPACITY_INACTIVE = 0.6
			m_OPACITY_FAST = 100
			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (7 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String, _
			ByVal _ReturnedInput As System.Boolean, _
			ByVal _OPACITY_ACTIVE As System.Double _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput
			ReturnedInput = _ReturnedInput
			OPACITY_ACTIVE = _OPACITY_ACTIVE

			m_OPACITY_INACTIVE = 0.6
			m_OPACITY_FAST = 100
			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (8 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String, _
			ByVal _ReturnedInput As System.Boolean, _
			ByVal _OPACITY_ACTIVE As System.Double, _
			ByVal _OPACITY_INACTIVE As System.Double _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput
			ReturnedInput = _ReturnedInput
			OPACITY_ACTIVE = _OPACITY_ACTIVE
			OPACITY_INACTIVE = _OPACITY_INACTIVE

			m_OPACITY_FAST = 100
			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (9 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String, _
			ByVal _ReturnedInput As System.Boolean, _
			ByVal _OPACITY_ACTIVE As System.Double, _
			ByVal _OPACITY_INACTIVE As System.Double, _
			ByVal _OPACITY_THREAD As System.Threading.Thread _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput
			ReturnedInput = _ReturnedInput
			OPACITY_ACTIVE = _OPACITY_ACTIVE
			OPACITY_INACTIVE = _OPACITY_INACTIVE
			OPACITY_THREAD = _OPACITY_THREAD

			m_OPACITY_FAST = 100
			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (10 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String, _
			ByVal _ReturnedInput As System.Boolean, _
			ByVal _OPACITY_ACTIVE As System.Double, _
			ByVal _OPACITY_INACTIVE As System.Double, _
			ByVal _OPACITY_THREAD As System.Threading.Thread, _
			ByVal _OPACITY_FAST As System.Int32 _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput
			ReturnedInput = _ReturnedInput
			OPACITY_ACTIVE = _OPACITY_ACTIVE
			OPACITY_INACTIVE = _OPACITY_INACTIVE
			OPACITY_THREAD = _OPACITY_THREAD
			OPACITY_FAST = _OPACITY_FAST

			m_OPACITY_SLOW = 1500
			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (11 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String, _
			ByVal _ReturnedInput As System.Boolean, _
			ByVal _OPACITY_ACTIVE As System.Double, _
			ByVal _OPACITY_INACTIVE As System.Double, _
			ByVal _OPACITY_THREAD As System.Threading.Thread, _
			ByVal _OPACITY_FAST As System.Int32, _
			ByVal _OPACITY_SLOW As System.Int32 _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput
			ReturnedInput = _ReturnedInput
			OPACITY_ACTIVE = _OPACITY_ACTIVE
			OPACITY_INACTIVE = _OPACITY_INACTIVE
			OPACITY_THREAD = _OPACITY_THREAD
			OPACITY_FAST = _OPACITY_FAST
			OPACITY_SLOW = _OPACITY_SLOW

			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (12 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String, _
			ByVal _ReturnedInput As System.Boolean, _
			ByVal _OPACITY_ACTIVE As System.Double, _
			ByVal _OPACITY_INACTIVE As System.Double, _
			ByVal _OPACITY_THREAD As System.Threading.Thread, _
			ByVal _OPACITY_FAST As System.Int32, _
			ByVal _OPACITY_SLOW As System.Int32, _
			ByVal _Window_Locked As System.Boolean _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput
			ReturnedInput = _ReturnedInput
			OPACITY_ACTIVE = _OPACITY_ACTIVE
			OPACITY_INACTIVE = _OPACITY_INACTIVE
			OPACITY_THREAD = _OPACITY_THREAD
			OPACITY_FAST = _OPACITY_FAST
			OPACITY_SLOW = _OPACITY_SLOW
			Window_Locked = _Window_Locked

			m_COLOUR_UNLOCKED_BORDER = Windows.Media.Color.FromArgb(255, 26, 26, 26)
			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (13 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String, _
			ByVal _ReturnedInput As System.Boolean, _
			ByVal _OPACITY_ACTIVE As System.Double, _
			ByVal _OPACITY_INACTIVE As System.Double, _
			ByVal _OPACITY_THREAD As System.Threading.Thread, _
			ByVal _OPACITY_FAST As System.Int32, _
			ByVal _OPACITY_SLOW As System.Int32, _
			ByVal _Window_Locked As System.Boolean, _
			ByVal _COLOUR_UNLOCKED_BORDER As System.Windows.Media.Color _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput
			ReturnedInput = _ReturnedInput
			OPACITY_ACTIVE = _OPACITY_ACTIVE
			OPACITY_INACTIVE = _OPACITY_INACTIVE
			OPACITY_THREAD = _OPACITY_THREAD
			OPACITY_FAST = _OPACITY_FAST
			OPACITY_SLOW = _OPACITY_SLOW
			Window_Locked = _Window_Locked
			COLOUR_UNLOCKED_BORDER = _COLOUR_UNLOCKED_BORDER

			m_COLOUR_LOCKED_BORDER = Windows.Media.Color.FromArgb(255, 52, 26, 26)
			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (14 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String, _
			ByVal _ReturnedInput As System.Boolean, _
			ByVal _OPACITY_ACTIVE As System.Double, _
			ByVal _OPACITY_INACTIVE As System.Double, _
			ByVal _OPACITY_THREAD As System.Threading.Thread, _
			ByVal _OPACITY_FAST As System.Int32, _
			ByVal _OPACITY_SLOW As System.Int32, _
			ByVal _Window_Locked As System.Boolean, _
			ByVal _COLOUR_UNLOCKED_BORDER As System.Windows.Media.Color, _
			ByVal _COLOUR_LOCKED_BORDER As System.Windows.Media.Color _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput
			ReturnedInput = _ReturnedInput
			OPACITY_ACTIVE = _OPACITY_ACTIVE
			OPACITY_INACTIVE = _OPACITY_INACTIVE
			OPACITY_THREAD = _OPACITY_THREAD
			OPACITY_FAST = _OPACITY_FAST
			OPACITY_SLOW = _OPACITY_SLOW
			Window_Locked = _Window_Locked
			COLOUR_UNLOCKED_BORDER = _COLOUR_UNLOCKED_BORDER
			COLOUR_LOCKED_BORDER = _COLOUR_LOCKED_BORDER

			InitializeComponent()

		End Sub

		''' <summary>Parametered Constructor (15 Parameters)</summary>
		Public Sub New( _
			ByVal _Command As System.String, _
			ByVal _Id As System.Int32, _
			ByVal _Completed As System.Boolean, _
			ByVal _LastProgress As System.Single, _
			ByVal _ReturnInput As System.String, _
			ByVal _ReturnedInput As System.Boolean, _
			ByVal _OPACITY_ACTIVE As System.Double, _
			ByVal _OPACITY_INACTIVE As System.Double, _
			ByVal _OPACITY_THREAD As System.Threading.Thread, _
			ByVal _OPACITY_FAST As System.Int32, _
			ByVal _OPACITY_SLOW As System.Int32, _
			ByVal _Window_Locked As System.Boolean, _
			ByVal _COLOUR_UNLOCKED_BORDER As System.Windows.Media.Color, _
			ByVal _COLOUR_LOCKED_BORDER As System.Windows.Media.Color, _
			ByVal _Opacity_Locked As System.Boolean _
		)

			MyBase.New()

			Command = _Command
			Id = _Id
			Completed = _Completed
			LastProgress = _LastProgress
			ReturnInput = _ReturnInput
			ReturnedInput = _ReturnedInput
			OPACITY_ACTIVE = _OPACITY_ACTIVE
			OPACITY_INACTIVE = _OPACITY_INACTIVE
			OPACITY_THREAD = _OPACITY_THREAD
			OPACITY_FAST = _OPACITY_FAST
			OPACITY_SLOW = _OPACITY_SLOW
			Window_Locked = _Window_Locked
			COLOUR_UNLOCKED_BORDER = _COLOUR_UNLOCKED_BORDER
			COLOUR_LOCKED_BORDER = _COLOUR_LOCKED_BORDER
			Opacity_Locked = _Opacity_Locked

			InitializeComponent()

		End Sub

	#End Region

	#Region " Class Plumbing/Interface Code "

		#Region " Thread-Safe Updates Implementation "

			#Region " Private Variables "

				''' <summary></summary>
				''' <remarks></remarks>
				Private CurrentCharacterWidth_LOCK As New System.Object

				''' <summary></summary>
				''' <remarks></remarks>
				Private CurrentCharacterWidth_HASVALUE As System.Boolean

			#End Region

			#Region " Public Update Methods "

			#End Region

		#End Region

	#End Region

	#Region " Public Constants "

		''' <summary>Public Shared Reference to the Name of the Property: Command</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_COMMAND As String = "Command"

		''' <summary>Public Shared Reference to the Name of the Property: Id</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_ID As String = "Id"

		''' <summary>Public Shared Reference to the Name of the Property: Completed</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_COMPLETED As String = "Completed"

		''' <summary>Public Shared Reference to the Name of the Property: LastProgress</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_LASTPROGRESS As String = "LastProgress"

		''' <summary>Public Shared Reference to the Name of the Property: ReturnInput</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_RETURNINPUT As String = "ReturnInput"

		''' <summary>Public Shared Reference to the Name of the Property: ReturnedInput</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_RETURNEDINPUT As String = "ReturnedInput"

		''' <summary>Public Shared Reference to the Name of the Property: OPACITY_ACTIVE</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_OPACITY_ACTIVE As String = "OPACITY_ACTIVE"

		''' <summary>Public Shared Reference to the Name of the Property: OPACITY_INACTIVE</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_OPACITY_INACTIVE As String = "OPACITY_INACTIVE"

		''' <summary>Public Shared Reference to the Name of the Property: OPACITY_THREAD</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_OPACITY_THREAD As String = "OPACITY_THREAD"

		''' <summary>Public Shared Reference to the Name of the Property: OPACITY_FAST</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_OPACITY_FAST As String = "OPACITY_FAST"

		''' <summary>Public Shared Reference to the Name of the Property: OPACITY_SLOW</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_OPACITY_SLOW As String = "OPACITY_SLOW"

		''' <summary>Public Shared Reference to the Name of the Property: Window_Locked</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_WINDOW_LOCKED As String = "Window_Locked"

		''' <summary>Public Shared Reference to the Name of the Property: COLOUR_UNLOCKED_BORDER</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_COLOUR_UNLOCKED_BORDER As String = "COLOUR_UNLOCKED_BORDER"

		''' <summary>Public Shared Reference to the Name of the Property: COLOUR_LOCKED_BORDER</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_COLOUR_LOCKED_BORDER As String = "COLOUR_LOCKED_BORDER"

		''' <summary>Public Shared Reference to the Name of the Property: Opacity_Locked</summary>
		''' <remarks></remarks>
		Public Const PROPERTY_OPACITY_LOCKED As String = "Opacity_Locked"

	#End Region

	#Region " Private Variables "

		''' <summary>Private Data Storage Variable for Property: Command</summary>
		''' <remarks></remarks>
		Private m_Command As System.String

		''' <summary>Private Data Storage Variable for Property: Id</summary>
		''' <remarks></remarks>
		Private m_Id As System.Int32

		''' <summary>Private Data Storage Variable for Property: Completed</summary>
		''' <remarks></remarks>
		Private m_Completed As System.Boolean

		''' <summary>Private Data Storage Variable for Property: LastProgress</summary>
		''' <remarks></remarks>
		Private m_LastProgress As System.Single

		''' <summary>Private Data Storage Variable for Property: ReturnInput</summary>
		''' <remarks></remarks>
		Private m_ReturnInput As System.String

		''' <summary>Private Data Storage Variable for Property: ReturnedInput</summary>
		''' <remarks></remarks>
		Private m_ReturnedInput As System.Boolean

		''' <summary>Private Data Storage Variable for Property: OPACITY_ACTIVE</summary>
		''' <remarks></remarks>
		Private m_OPACITY_ACTIVE As System.Double

		''' <summary>Private Data Storage Variable for Property: OPACITY_INACTIVE</summary>
		''' <remarks></remarks>
		Private m_OPACITY_INACTIVE As System.Double

		''' <summary>Private Data Storage Variable for Property: OPACITY_THREAD</summary>
		''' <remarks></remarks>
		Private m_OPACITY_THREAD As System.Threading.Thread

		''' <summary>Private Data Storage Variable for Property: OPACITY_FAST</summary>
		''' <remarks></remarks>
		Private m_OPACITY_FAST As System.Int32

		''' <summary>Private Data Storage Variable for Property: OPACITY_SLOW</summary>
		''' <remarks></remarks>
		Private m_OPACITY_SLOW As System.Int32

		''' <summary>Private Data Storage Variable for Property: Window_Locked</summary>
		''' <remarks></remarks>
		Private m_Window_Locked As System.Boolean

		''' <summary>Private Data Storage Variable for Property: COLOUR_UNLOCKED_BORDER</summary>
		''' <remarks></remarks>
		Private m_COLOUR_UNLOCKED_BORDER As System.Windows.Media.Color

		''' <summary>Private Data Storage Variable for Property: COLOUR_LOCKED_BORDER</summary>
		''' <remarks></remarks>
		Private m_COLOUR_LOCKED_BORDER As System.Windows.Media.Color

		''' <summary>Private Data Storage Variable for Property: Opacity_Locked</summary>
		''' <remarks></remarks>
		Private m_Opacity_Locked As System.Boolean

	#End Region

	#Region " Public Properties "

		''' <summary>Provides Access to the Property: Command</summary>
		''' <remarks></remarks>
		Public Property Command() As System.String
			Get
				Return m_Command
			End Get
			Set(value As System.String)
				m_Command = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: Id</summary>
		''' <remarks></remarks>
		Public Property Id() As System.Int32
			Get
				Return m_Id
			End Get
			Set(value As System.Int32)
				m_Id = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: Completed</summary>
		''' <remarks></remarks>
		Public Property Completed() As System.Boolean
			Get
				Return m_Completed
			End Get
			Set(value As System.Boolean)
				m_Completed = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: LastProgress</summary>
		''' <remarks></remarks>
		Public Property LastProgress() As System.Single
			Get
				Return m_LastProgress
			End Get
			Set(value As System.Single)
				m_LastProgress = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: ReturnInput</summary>
		''' <remarks></remarks>
		Public Property ReturnInput() As System.String
			Get
				Return m_ReturnInput
			End Get
			Set(value As System.String)
				m_ReturnInput = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: ReturnedInput</summary>
		''' <remarks></remarks>
		Public Property ReturnedInput() As System.Boolean
			Get
				Return m_ReturnedInput
			End Get
			Set(value As System.Boolean)
				m_ReturnedInput = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: OPACITY_ACTIVE</summary>
		''' <remarks></remarks>
		Public Property OPACITY_ACTIVE() As System.Double
			Get
				Return m_OPACITY_ACTIVE
			End Get
			Set(value As System.Double)
				m_OPACITY_ACTIVE = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: OPACITY_INACTIVE</summary>
		''' <remarks></remarks>
		Public Property OPACITY_INACTIVE() As System.Double
			Get
				Return m_OPACITY_INACTIVE
			End Get
			Set(value As System.Double)
				m_OPACITY_INACTIVE = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: OPACITY_THREAD</summary>
		''' <remarks></remarks>
		Public Property OPACITY_THREAD() As System.Threading.Thread
			Get
				Return m_OPACITY_THREAD
			End Get
			Set(value As System.Threading.Thread)
				m_OPACITY_THREAD = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: OPACITY_FAST</summary>
		''' <remarks></remarks>
		Public Property OPACITY_FAST() As System.Int32
			Get
				Return m_OPACITY_FAST
			End Get
			Set(value As System.Int32)
				m_OPACITY_FAST = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: OPACITY_SLOW</summary>
		''' <remarks></remarks>
		Public Property OPACITY_SLOW() As System.Int32
			Get
				Return m_OPACITY_SLOW
			End Get
			Set(value As System.Int32)
				m_OPACITY_SLOW = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: Window_Locked</summary>
		''' <remarks></remarks>
		Public Property Window_Locked() As System.Boolean
			Get
				Return m_Window_Locked
			End Get
			Set(value As System.Boolean)
				m_Window_Locked = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: COLOUR_UNLOCKED_BORDER</summary>
		''' <remarks></remarks>
		Public Property COLOUR_UNLOCKED_BORDER() As System.Windows.Media.Color
			Get
				Return m_COLOUR_UNLOCKED_BORDER
			End Get
			Set(value As System.Windows.Media.Color)
				m_COLOUR_UNLOCKED_BORDER = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: COLOUR_LOCKED_BORDER</summary>
		''' <remarks></remarks>
		Public Property COLOUR_LOCKED_BORDER() As System.Windows.Media.Color
			Get
				Return m_COLOUR_LOCKED_BORDER
			End Get
			Set(value As System.Windows.Media.Color)
				m_COLOUR_LOCKED_BORDER = value
			End Set
		End Property

		''' <summary>Provides Access to the Property: Opacity_Locked</summary>
		''' <remarks></remarks>
		Public Property Opacity_Locked() As System.Boolean
			Get
				Return m_Opacity_Locked
			End Get
			Set(value As System.Boolean)
				m_Opacity_Locked = value
			End Set
		End Property

	#End Region

End Class