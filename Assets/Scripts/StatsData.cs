public class StatsData{
	public int dinero;
	public int comida;
	public int aldeanosTotal;
	public int aldeanosEnfermos;
	public int aldeanosHambrientos;
	public BuildingsCreated[] _totalBuildings;

	public StatsData () {
		this.dinero = 0;
		this.comida = 0;
		this.aldeanosTotal = 0;
		this.aldeanosEnfermos = 0;
		this.aldeanosHambrientos = 0;
		this._totalBuildings = new BuildingsCreated[0];
	}
}