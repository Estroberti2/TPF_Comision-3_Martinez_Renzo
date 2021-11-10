
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
namespace DeepSpace
{

	class Estrategia
	{
		
		
		/*
*--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 	
  			Calcula y retorna un texto 
			- con la distancia del camino que existe entre el planeta del Bot y la raíz
		*/
		public String Consulta1( ArbolGeneral<Planeta> arbol)
		{
			// creo e instancio las listas
			List<ArbolGeneral<Planeta>> camino = new List<ArbolGeneral<Planeta>>();
			List<ArbolGeneral<Planeta>> mejor_camino = new List<ArbolGeneral<Planeta>>();
			
			// llamo al metodo
			_RaizIA(arbol, camino, mejor_camino);
			
			string mejor = "Camino a recorer: ";
			
			// recorro la lista y voy adiriendo al string la poblacion de los diferentes nodos
			foreach(ArbolGeneral<Planeta> p in mejor_camino)
			{
				mejor += p.getDatoRaiz().population + " ";
			}
			// adiciono al string la distancia
			mejor += " \tDistancia: " + (mejor_camino.Count - 1);
			
			// retorno el string
			return mejor;	
		}
		
		// revisa el arbol
		// devuelve el camino desde la raiz hasta la ia
		public List<ArbolGeneral<Planeta>> _RaizIA(ArbolGeneral<Planeta> arbol, List<ArbolGeneral<Planeta>> camino, List<ArbolGeneral<Planeta>> mejor_camino)
		{
			// agrego el arbol al camino
			camino.Add(arbol);
			
			// verifico si es ia
			if(arbol.getDatoRaiz().team == 2)
			{
				// si es asi limpio mejor_camino y le agrego camino 
				mejor_camino.Clear();
				foreach(ArbolGeneral<Planeta> p in camino)
				{
					mejor_camino.Add(p);
				}
			}
			// si no es ia
			else
			{
				//verificamos si tiene hijos
				if(arbol.getHijos().Count > 0)
				{
					// recorremos sus hijos
					foreach(ArbolGeneral<Planeta> planetas in arbol.getHijos())
					{
						// rellamamos
						_RaizIA(planetas, camino, mejor_camino);
						// eliminamos el ultimo elemento del camino
						camino.RemoveAt(camino.Count - 1);
					}
				}
			}
			// retornamos mejor_camino
			return mejor_camino;
		}


		/*
*--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
  			Retorna un texto con el listado de 
			- los planetas ubicados en todos los descendientes del nodo que contiene al planeta del Bot.
		*/
		public String Consulta2( ArbolGeneral<Planeta> arbol)
		{
			List<ArbolGeneral<Planeta>> camino = new List<ArbolGeneral<Planeta>>();
			
			_Consulta2(arbol, camino);
			
			string hijos = "Lista Hijos IA: ";
			
			foreach(ArbolGeneral<Planeta> p in camino){
				hijos += p.getDatoRaiz().population + ", ";
			}
			
			hijos += " \tCant. Hijos IA: " + (camino.Count);
			
			return hijos;
		}
		
		// retorna la lista de hijos desde nodo ia hasta nodos hojas
		// para este me base en la formula inverza al del inciso anterior
		public List<ArbolGeneral<Planeta>> _Consulta2(ArbolGeneral<Planeta> arbol, List<ArbolGeneral<Planeta>> camino)
		{
			// si arbol es ia
			if(arbol.getDatoRaiz().EsPlanetaDeLaIA())
			{
				// si no es hoja
				if(!arbol.esHoja())
				{
					// recorro sus hijos
					foreach(ArbolGeneral<Planeta> planetas in arbol.getHijos())
					{
						// los guardo en la lista
						camino.Add(planetas);
						
						// si el hijo no es hoja
						if(!planetas.esHoja())
						{
							// recorro los hijos de los hijos
							foreach(ArbolGeneral<Planeta> planeta in planetas.getHijos())
							{
								// y los guardo en la lista
								camino.Add(planeta);
							}
						}
					}
				}
			}
			// sino, mientras tenga hijos se rellamara
			else{
				if(arbol.getHijos().Count > 0)
				{
					foreach(ArbolGeneral<Planeta> planetas in arbol.getHijos())
					{
						_Consulta2(planetas, camino);
					}
				}
			}
			// retorna la lista
			return camino;
		}
		
		/*
*--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
			Calcula y retorna en un texto 
			- con la población total y promedio por cada nivel del árbol
		*/
		public String Consulta3( ArbolGeneral<Planeta> arbol)
		{
			// creo e instancio una cola y una lista
			
			// la cola la uso para ir recorriendo el arbol
			Cola<ArbolGeneral<Planeta>> nivel = new Cola<ArbolGeneral<Planeta>>();
			
			// la lista para ir guardando los diferentes nodos 
			List<ArbolGeneral<Planeta>> nuevo_nivel = new List<ArbolGeneral<Planeta>>();
			
			// instancio un arbol con null 
			ArbolGeneral<Planeta> auxiliar = null;
			
			// guardo el arbol en la cola y en la lista
			nivel.encolar(arbol);
			nuevo_nivel.Add(arbol);
			
			// instanciamos un boleano en true 
			bool aux = true;
			
			// mientras sea true
			while(aux)
			{
				// desencolamos cola en auxiliar
				auxiliar = nivel.desencolar();
				
				// mientras auxiliar no sea hoja
				if(!auxiliar.esHoja())
				{
					// recorremos sus hijos y lo agregamos a cola y lista
					foreach(ArbolGeneral<Planeta> p in auxiliar.getHijos())
					{
						nivel.encolar(p);
						nuevo_nivel.Add(p);
					}
				}
				// si es hoja marca false
				else{
					aux = false;
				}
			}
			// ya con  todo lo anterior nos queda la lista con todos los nodos
			
			// lo siguiente sera recorrer esa lista por etapas y luego agregar en 
			// el string su nivel, nodosdel nivel y total del nivel
			
			string texto = "";
			int total = 0; // poblacion en nivel
			int total_total = 0; // poblacion en todo el arbol
			int nivel_actual = 0; // nivel recorrido
			int hijos_nivel = 0; // hijos del nivel actual
			int hijos_prox_lvl = 0; //hijos del proximo nivel
			int tope = 0; // marca cuando se termina el nivel actual
			
			// en resumen, en cada instancia suma los hijos del proximo nivel, el total y el total_toal
			// cuando llega al tope 
			// 		le sumo los hijos del proximo nivel
			//		reemplazo hijos nivel con los del proximo nivel
			//		pongo en cero a total y a hijos proximo nivel
			for(int  i = 0; i < nuevo_nivel.Count; i++)
			{
				if(nuevo_nivel[i].getHijos().Count > 0)
				{
					if(nivel_actual == 0)
					{
						tope += nuevo_nivel[i].getHijos().Count;
						texto += "Nivel: " + nivel_actual + "\tProm. Pobl. Nivel: " + nuevo_nivel[i].getDatoRaiz().population + "\tCant. Nodos: 1" + "\tPobl. Tot. Nivel: " + nuevo_nivel[i].getDatoRaiz().population + "\n";
						nivel_actual++;
						hijos_nivel = nuevo_nivel[i].getHijos().Count;
						total_total += Convert.ToInt16(nuevo_nivel[i].getDatoRaiz().population);
					}
					if(i >=1 &&  i <= tope)
					{
						hijos_prox_lvl += nuevo_nivel[i].getHijos().Count;
						total += Convert.ToInt16(nuevo_nivel[i].getDatoRaiz().population);
						total_total += Convert.ToInt16(nuevo_nivel[i].getDatoRaiz().population);
					}
					if(i == tope)
					{
						tope += hijos_prox_lvl;
						texto += "Nivel: " + nivel_actual + "\tProm Pobl Nivel:" + (total/hijos_nivel) + "\tCant. Nodos: " + hijos_nivel + "\tPobl. Tot. Nivel: " + total + "\n";
						nivel_actual++;
						hijos_nivel = hijos_prox_lvl;
						hijos_prox_lvl = 0;
						total = 0;
					}
				}
				else
				{
					total += Convert.ToInt16(nuevo_nivel[i].getDatoRaiz().population);
					total_total += Convert.ToInt16(nuevo_nivel[i].getDatoRaiz().population);
					if(i == (nuevo_nivel.Count - 1))
					{
						texto += "Nivel: " + nivel_actual + "\tProm Pobl Nivel:" + (total/hijos_nivel) + "\tCant. Nodos: " + hijos_nivel + "\tPobl. Tot. Nivel: " + total + "\n";
					}
				}
			}
			//texto += "Poblacion total: " + total_total;
			return texto;
		}
	
		
		/*
*--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- 
			Este método calcula y retorna el movimiento apropiado según el estado del juego. 
			El estado del juego es recibido en el parámetro arbol de tipo ArbolGeneral<Planeta>. 
			Cada nodo del árbol contiene como dato un planeta del juego
		 */
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{
			List<ArbolGeneral<Planeta>> recorrido = _CalcularMovimiento(arbol);
			
			Planeta o = null;
			Planeta d = null;
			// reviso si la poblacion es mayor a la poblacion del objetivo
			if(recorrido[0].getDatoRaiz().population > (recorrido[1].getDatoRaiz().population * 1.2)){
				o = recorrido[0].getDatoRaiz();
				d = recorrido[1].getDatoRaiz();
			}
			// sino pido ayuda del anterior
			else{
				for(int i = 0; i < recorrido.Count; i++){
					if(recorrido[i].getDatoRaiz().EsPlanetaDelJugador() && recorrido[i-1].getDatoRaiz().EsPlanetaDeLaIA()){
						if(i >= 1){
							o = recorrido[i-1].getDatoRaiz();
							d = recorrido[i].getDatoRaiz();
						}
					}
				}
			}
			// creo y paso movimiento
			Movimiento mov = new Movimiento(o, d);
			return mov;
		}
		
		// busca camino de raiz a ia y de raiz a jugador
		// retorna el recorrido
		public List<ArbolGeneral<Planeta>> _CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{
			// creo las diferentes listas a usar
			
			// listas para el camino de la ia
			List<ArbolGeneral<Planeta>> camino_ia = new List<ArbolGeneral<Planeta>>();
			List<ArbolGeneral<Planeta>> mejor_camino_ia = new List<ArbolGeneral<Planeta>>();
			
			// como para el camino del jugador
			List<ArbolGeneral<Planeta>> camino_jugador = new List<ArbolGeneral<Planeta>>();
			List<ArbolGeneral<Planeta>> mejor_camino_jugador = new List<ArbolGeneral<Planeta>>();
			
			// esta lista es para el recorrido de la ia al jugador
			List<ArbolGeneral<Planeta>> recorrido = new List<ArbolGeneral<Planeta>>();
			
			// luego lo que hare sera buscar el camino desde raiz hacia ambos
			_RaizIA(arbol, camino_ia, mejor_camino_ia);
			_RaizAJugador(arbol, camino_jugador, mejor_camino_jugador);
			
			// instanciamos arboles vacios para usarlos auxiliarmente
			ArbolGeneral<Planeta> ia = null;
			ArbolGeneral<Planeta> jugador = null;
			
			// luego verificar si uno es padre del otro y retornar de acuerdo a ello
			
			// verificamos si jugador es padre de ia
			// y almacenamos a ia
			bool es_hijo_jugador = false;
			foreach(ArbolGeneral<Planeta> p in mejor_camino_jugador[mejor_camino_jugador.Count -1].getHijos()){
				if(p.getDatoRaiz().EsPlanetaDeLaIA()){
					es_hijo_jugador = true;
					ia = p;
				}
			}
			// si es asi almacenamos en recorrido a ia y luego jugador
			if(es_hijo_jugador){
				recorrido.Add(ia);
				recorrido.Add(mejor_camino_jugador[mejor_camino_jugador.Count -1]);
			}
			
			// verificamos si ia es padre de jugador
			// y almacenamos a jugador
			bool es_hijo_ia = false;
			foreach(ArbolGeneral<Planeta> p in mejor_camino_ia[mejor_camino_ia.Count -1].getHijos()){
				if(p.getDatoRaiz().EsPlanetaDelJugador()){
					es_hijo_ia = true;
					jugador = p;
				}
			}
			// si es asi almacenamos en recorrido a jugador y luego a ia
			if(es_hijo_ia){
				recorrido.Add(mejor_camino_ia[mejor_camino_ia.Count -1]);
				recorrido.Add(jugador);
			}
			
			// en caso de que ninguno sea hijo de otro
			// almacenamos mejor_camino_ia en recorrido
			// luego almacenamos los datos no repetidos de mejor_camino_jugador a recorrido
			if(!es_hijo_ia && !es_hijo_jugador){
				mejor_camino_ia.Reverse();
				foreach(ArbolGeneral<Planeta> p in mejor_camino_ia){
					recorrido.Add(p);
				}
				foreach(ArbolGeneral<Planeta> p in mejor_camino_jugador){
						if(!mejor_camino_ia.Contains(p)){
							recorrido.Add(p);
						}
				}
			}
			// retornamos el recorrido
			return recorrido;
		}

		
		// simplemente modifique _Consulta1 para que de el camino de raiz hasta jugador
		public List<ArbolGeneral<Planeta>> _RaizAJugador(ArbolGeneral<Planeta> arbol, List<ArbolGeneral<Planeta>> camino, List<ArbolGeneral<Planeta>> mejor_camino)
		{
			int contador = 0;
			camino.Add(arbol);
			if(arbol.getDatoRaiz().team == 1)
			{
				mejor_camino.Clear();
				foreach(ArbolGeneral<Planeta> p in camino)
				{
					mejor_camino.Add(p);
				}
			}
			else
			{
				if(arbol.getHijos().Count > 0)
				{
					foreach(ArbolGeneral<Planeta> planetas in arbol.getHijos())
					{
						contador++;
						_RaizAJugador(planetas, camino, mejor_camino);
						camino.RemoveAt(camino.Count - 1);
					}
				}
			}
			return mejor_camino;
		}
	}
}
