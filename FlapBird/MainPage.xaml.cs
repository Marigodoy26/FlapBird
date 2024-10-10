namespace FlapBird;

public partial class MainPage : ContentPage
{
	const int gravidade = 1;
	const int TempoEntreFrames = 25;
	bool EstaMorto = false;

	public MainPage()
	{
		InitializeComponent();

		//Desenhar().RunSynchronously();
	}
	void AplicaGravidade()
	{
		ImagePassarinho.TranslationY += gravidade;
	}

	async Task Desenhar()
	{
		while (!EstaMorto)
		{
			AplicaGravidade();
			await Task.Delay(TempoEntreFrames);
		}
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		Desenhar();
    }
}

