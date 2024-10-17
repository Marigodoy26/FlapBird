namespace FlapBird;

public partial class MainPage : ContentPage
{
	const int gravidade = 2;
	const int TempoEntreFrames = 25;
	bool EstaMorto = true;
	double LarguraJanela = 0;
	double AlturaJanela = 0;
	int Velocidade = 20;
	const int ForcaPulo = 20;
	bool EstaPulando = false;
	int TempoPulando = 1;
	const int maxTempoPulando = 3;
	const int aberturaMinima = 400;
	int Score=0;


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
			if (EstaPulando)
				AplicaPulo();
			else
				AplicaGravidade();
			await Task.Delay(TempoEntreFrames);
			GerenciaCanos();
			if (VerificaColisao())
			{
				EstaMorto = true;
				FrameGameOver.IsVisible = true;
				break;
			}
			await Task.Delay(TempoEntreFrames);
		}
	}


	void OnGameOverClicked(object s, TappedEventArgs a)
	{
		FrameGameOver.IsVisible = false;
		Inicializar();
		Desenhar();
	}

	void Inicializar()
	{
		EstaMorto = false;
		ImagePassarinho.TranslationY = 0;
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);
		LarguraJanela = width;
		AlturaJanela = height;
	}

	void GerenciaCanos()
	{
		CanoCima.TranslationX -= Velocidade;
		CanoBaixo.TranslationX -= Velocidade;
		if (CanoBaixo.TranslationX <= -LarguraJanela)
		{
			CanoCima.TranslationX = 0;
			CanoBaixo.TranslationX = 0;
			var alturaMax = -100;
			var alturaMin = -CanoBaixo.HeightRequest;
			CanoCima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			CanoBaixo.TranslationY = CanoCima.TranslationY + aberturaMinima + CanoBaixo.HeightRequest;
			Score++;
			labelScore.Text="Canos:"+Score.ToString("D3");
		}
	}

	bool VerificaColisaoTeto()
	{
		var minY = -AlturaJanela / 2;
		if (ImagePassarinho.TranslationY <= minY)
			return true;
		else
			return false;
	}

	bool VerificaColisaoChao()
	{
		var maxY = AlturaJanela / 2 - Chao.HeightRequest;
		if (ImagePassarinho.TranslationY >= maxY)
			return true;
		else
			return false;
	}

	bool VerificaColisao()
	{
		if (!EstaMorto)
		{
			if (VerificaColisaoTeto() ||
			VerificaColisaoChao())
			{
				return true;
			}
		}
		return false;
	}

	void AplicaPulo()
	{
		ImagePassarinho.TranslationY -= ForcaPulo;
		TempoPulando++;
		if (TempoPulando >= maxTempoPulando)
		{
			EstaPulando = false;
			TempoPulando = 0;
		}
	}

	void OnGridClicked(object s, TappedEventArgs a)
	{
		EstaPulando = true;
	}

}

