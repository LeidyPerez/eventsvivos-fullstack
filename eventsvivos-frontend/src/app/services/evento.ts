import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventoService {
  private apiUrl = 'http://localhost:5023/api/eventos';

  constructor(private http: HttpClient) {}

  listarEventos(filtros?: any): Observable<any[]> {
    let params = new HttpParams();
    if (filtros?.tipo) params = params.set('tipo', filtros.tipo);
    if (filtros?.estado) params = params.set('estado', filtros.estado);
    if (filtros?.titulo) params = params.set('titulo', filtros.titulo);
    return this.http.get<any[]>(this.apiUrl, { params });
  }

  crearEvento(dto: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, dto);
  }

  crearReserva(dto: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/reservas`, dto);
  }

  confirmarPago(id: string): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/reservas/${id}/confirmar`, {});
  }

  cancelarReserva(id: string): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/reservas/${id}/cancelar`, {});
  }

  obtenerReporte(eventoId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${eventoId}/reporte`);
  }
}