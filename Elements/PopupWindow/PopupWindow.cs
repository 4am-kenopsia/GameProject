using Godot;
using MapGame;

public partial class PopupWindow : Node2D
{
	// Define custom signals
	[Signal]
	public delegate void ButtonPressed1EventHandler();

	[Signal]
	public delegate void ButtonPressed2EventHandler();

	private Button _button1;
	private Button _button2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_button1 = GetNode<Button>("Panel/Button");
		_button2 = GetNode<Button>("Panel/Button2");

		// Connect the button's "pressed" signal to local methods
		_button1.Pressed += OnButton1Pressed;
		_button2.Pressed += OnButton2Pressed;
	}

	private void OnButton1Pressed()
	{
		// Emit the custom signal
		ResourceManager.Instance.IncreaseThirdResource(1);
		EmitSignal(SignalName.ButtonPressed1);
		GD.Print("lol1");
	}

	private void OnButton2Pressed()
	{
		// Emit the custom signal
		ResourceManager.Instance.DecreaseMainResource(100);
		EmitSignal(SignalName.ButtonPressed2);
		GD.Print("lol2");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
