using System;

public enum Jugada
{
	Piedra,
	Papel,
	Tijeras
}

public class Jugador
{
	public string Nombre { get; }

	public Jugador(string nombre)
	{
		Nombre = nombre;
	}

	public virtual Jugada RealizarJugada()
	{
		Console.WriteLine($"{Nombre}, elige tu jugada (1-Piedra, 2-Papel, 3-Tijeras):");
		int opcion = Convert.ToInt32(Console.ReadLine());

		// Validar la entrada del jugador
		while (opcion < 1 || opcion > 3)
		{
			Console.WriteLine("Jugada inválida. Elige nuevamente:");
			opcion = Convert.ToInt32(Console.ReadLine());
		}

		// Convertir la opción seleccionada en una jugada
		return (Jugada)(opcion - 1);
	}
}

public class JugadorMaquina : Jugador
{
	private Random random;

	public JugadorMaquina(string nombre) : base(nombre)
	{
		random = new Random();
	}

	public override Jugada RealizarJugada()
	{
		int opcion = random.Next(1, 4);
		return (Jugada)(opcion - 1);
	}
}

public class Juego
{
	private Jugador _jugador1;
	private Jugador _jugador2;

	private static readonly string[,] resultados =
	{
		{ "Empate", "La máquina 🤖 ganó", "Usted ganó " },
		{ "Usted ganó", "Empate", "La máquina 🤖 ganó" },
		{ "La máquina 🤖 ganó", "Usted ganó", "Empate" }
	};

	public Juego(Jugador jugador1, Jugador jugador2)
	{
		_jugador1 = jugador1;
		_jugador2 = jugador2;
	}

	public void Jugar()
	{
		Jugada jugadaJugador1 = _jugador1.RealizarJugada();
		Jugada jugadaJugador2 = _jugador2.RealizarJugada();

		Console.WriteLine($"{_jugador1.Nombre}: {jugadaJugador1}");
		Console.WriteLine($"{_jugador2.Nombre}: {jugadaJugador2}");

		Console.WriteLine(resultados[(int)jugadaJugador1, (int)jugadaJugador2]);
	}
}

public class Program
{
	public static void Main(string[] args)
	{
		Console.WriteLine("Bienvenido al juego Piedra, Papel y Tijeras");
		Console.Write("Ingresa tu nombre: ");

		Jugador jugador1 = new Jugador(Console.ReadLine() ?? "Jugador 1");
		// Jugador jugador2 = new Jugador(Console.ReadLine() ?? "Jugador 1");
		Jugador jugadorMaquina1 = new JugadorMaquina("Máquina 1");
		// Jugador jugadorMaquina2 = new JugadorMaquina("Máquina 2");


		Juego juego = new Juego(jugador1, jugadorMaquina1);
		juego.Jugar();

		Console.WriteLine("Presiona cualquier tecla para salir...");
		Console.ReadKey();
	}
}