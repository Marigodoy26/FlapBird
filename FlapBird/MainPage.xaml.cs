namespace FlapBird;

public partial class MainPage : ContentPage
{
	const int gravidade = 2;
	const int TempoEntreFrames = 20;
	bool EstaMorto = true;
	double LarguraJanela = 0;
	double AlturaJanela = 0;
	int Velocidade = 10;
	const int ForcaPulo = 20;
	bool EstaPulando = false;
	int TempoPulando = 1;
	const int maxTempoPulando = 3;
	const int aberturaMinima = 400;
	int Score = 0;


	public MainPage()
	{
		InitializeComponent();
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

			GerenciaCanos();

			if (VerificaColisao())
			{
				EstaMorto = true;
				FrameGameOver.IsVisible = true;
				labelScore.Text = "Você passou por\n" + Score + " canos";
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
		Score = 0;
		ImagePassarinho.TranslationY = 0;
		ImagePassarinho.TranslationX = 0;
		CanoCima.TranslationX = -LarguraJanela;
		CanoBaixo.TranslationX = -LarguraJanela;
		GerenciaCanos();
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);
		LarguraJanela = width;
		AlturaJanela = height;
		if (height > 0)
		{
			CanoCima.HeightRequest = height - Chao.HeightRequest;
			CanoBaixo.HeightRequest = height - Chao.HeightRequest;
		}
	}

	void GerenciaCanos()
	{
		CanoCima.TranslationX -= Velocidade;
		CanoBaixo.TranslationX -= Velocidade;
		if (CanoBaixo.TranslationX < -LarguraJanela)
		{
			CanoCima.TranslationX = 0;
			CanoBaixo.TranslationX = 0;

			var alturaMax =  -(CanoBaixo.HeightRequest * 0.1);
			var alturaMin = -(CanoBaixo.HeightRequest * 0.8);
			CanoCima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			CanoBaixo.TranslationY = CanoCima.TranslationY + aberturaMinima + CanoBaixo.HeightRequest;
		
			Score++;
			if (Score % 4 == 0)
				Velocidade++;
			labelScore.Text = "Canos:" + Score.ToString("D5");
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
			VerificaColisaoChao() ||
			VerificacolisaoCanoCima() ||
			VerificacolisaoCanoBaixo())
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

	bool VerificacolisaoCanoCima()
	{
		var posHPassarinho = (LarguraJanela - 50) - (ImagePassarinho.WidthRequest / 2);
		var posVPassarinho = (AlturaJanela / 2) - (ImagePassarinho.HeightRequest / 2) + ImagePassarinho.TranslationY;
		if
		(posHPassarinho >= Math.Abs(CanoCima.TranslationX) - CanoCima.WidthRequest &&
		posHPassarinho <= Math.Abs(CanoCima.TranslationX) + CanoCima.WidthRequest &&
		posHPassarinho <= CanoCima.HeightRequest + CanoCima.TranslationY)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	bool VerificacolisaoCanoBaixo()
	{
		var posHPassarinho = LarguraJanela - 50 - ImagePassarinho.WidthRequest / 2;
		var posVPassarinho = (AlturaJanela / 2) + (ImagePassarinho.HeightRequest / 2) + ImagePassarinho.TranslationY;
		var yMaxCano = CanoCima.HeightRequest + CanoCima.TranslationY + aberturaMinima;
		if
		(posHPassarinho >= Math.Abs(CanoCima.TranslationX) - CanoCima.WidthRequest &&
		posHPassarinho <= Math.Abs(CanoCima.TranslationX) + CanoCima.WidthRequest &&
		posVPassarinho >= yMaxCano)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}

