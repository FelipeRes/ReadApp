using System;

namespace ReadApp{
	public enum EstadoLeitura{
		Espera,
		Iniciado,
		Terminado
	}

	public class EstadoLeituraExtend{
		static public EstadoLeitura EstadoPorNome(string estado){
			if (estado == "Espera") {
				return EstadoLeitura.Espera;
			} else if (estado == "Iniciado") {
				return EstadoLeitura.Iniciado;
			} else if (estado == "Terminado") {
				return EstadoLeitura.Terminado;
			} else {
				return EstadoLeitura.Espera;
			}
		}
	}
}


