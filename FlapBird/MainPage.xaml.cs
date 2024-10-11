namespace FlapBird;

public partial class MainPage : ContentPage
{
	const int gravidade=5;
	const int TempoEntreFrames=25;
	bool EstaMorto=true;
	double LarguraJanela=0;
	double AlturaJanela=0;
	int Velocidade=20;


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
			GerenciaCanos();
		}
	}

    
	void OnGameOverClicked(object s, TappedEventArgs a)
	{
		FrameGameOver.IsVisible=false;
		Inicializar();
		Desenhar();
	}

	void Inicializar()
	{
		EstaMorto=false;
		ImagePassarinho.TranslationY=0;
	}

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
		LarguraJanela=width;
		AlturaJanela=height;
    }

	void GerenciaCanos()
	{
		CanoCima.TranslationX-=Velocidade;
		CanoBaixo.TranslationX-=Velocidade;
		if(CanoBaixo.TranslationX<=-LarguraJanela)
		{
		CanoCima.TranslationX=0;
		CanoBaixo.TranslationX=0;
		}
	}
}

