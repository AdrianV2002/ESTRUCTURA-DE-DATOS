import json
import os
from datetime import datetime
from typing import List, Dict
from colorama import init, Fore, Style

init(autoreset=True)

ARCHIVO_JSON = "turnos.json"

class Paciente:
    def __init__(self, cedula: str, nombre: str, especialidad: str, fecha: str, hora: str):
        self.cedula = cedula.strip()
        self.nombre = nombre.strip().title()
        self.especialidad = especialidad.strip().title()
        self.fecha = fecha
        self.hora = hora

    def __str__(self):
        return (f"{Fore.CYAN}{self.nombre}{Style.RESET_ALL} | "
                f"Cédula: {self.cedula} | "
                f"{self.especialidad} | "
                f"{self.fecha} {self.hora}")

    def to_dict(self):
        return {
            "cedula": self.cedula,
            "nombre": self.nombre,
            "especialidad": self.especialidad,
            "fecha": self.fecha,
            "hora": self.hora
        }

    @staticmethod
    def from_dict(data):
        return Paciente(data["cedula"], data["nombre"], data["especialidad"], data["fecha"], data["hora"])

class AgendaTurnos:
    def __init__(self):
        self.turnos: List[Paciente] = []
        self.cargar()

    def registrar_turno(self, paciente: Paciente) -> bool:
        for p in self.turnos:
            if p.fecha == paciente.fecha and p.hora == paciente.hora:
                return False
        self.turnos.append(paciente)
        self.guardar()
        return True

    def buscar_por_cedula(self, cedula: str) -> List[Paciente]:
        return [p for p in self.turnos if p.cedula == cedula.strip()]

    def turnos_por_fecha(self, fecha: str) -> List[Paciente]:
        return sorted([p for p in self.turnos if p.fecha == fecha], key=lambda x: x.hora)

    def cancelar_turno(self, cedula: str) -> bool:
        inicial = len(self.turnos)
        self.turnos = [p for p in self.turnos if p.cedula != cedula.strip()]
        cambio = len(self.turnos) < inicial
        if cambio:
            self.guardar()
        return cambio

    def reporte_por_especialidad(self) -> Dict[str, int]:
        conteo: Dict[str, int] = {}
        for p in self.turnos:
            conteo[p.especialidad] = conteo.get(p.especialidad, 0) + 1
        return conteo

    def guardar(self):
        with open(ARCHIVO_JSON, "w", encoding="utf-8") as f:
            json.dump([p.to_dict() for p in self.turnos], f, indent=4, ensure_ascii=False)

    def cargar(self):
        if os.path.exists(ARCHIVO_JSON):
            with open(ARCHIVO_JSON, "r", encoding="utf-8") as f:
                datos = json.load(f)
                self.turnos = [Paciente.from_dict(d) for d in datos]

class InterfazConsola:
    def __init__(self):
        self.agenda = AgendaTurnos()

    def mostrar_menu(self):
        while True:
            print(f"\n{Fore.GREEN}=== MENÚ PRINCIPAL - AGENDA DE TURNOS ==={Style.RESET_ALL}")
            print("1. Registrar nuevo turno")
            print("2. Consultar turno por cédula")
            print("3. Ver turnos por fecha")
            print("4. Cancelar turno")
            print("5. Reporte por especialidad")
            print("6. Salir")
            opcion = input("Seleccione una opción: ")

            match opcion:
                case "1": self.registrar()
                case "2": self.consultar_por_cedula()
                case "3": self.consultar_por_fecha()
                case "4": self.cancelar()
                case "5": self.reporte()
                case "6":
                    print("Cerrando sistema...")
                    break
                case _: print(f"{Fore.RED}Opción inválida.")

    def registrar(self):
        try:
            cedula = input("Cédula: ")
            nombre = input("Nombre completo: ")
            especialidad = input("Especialidad: ")
            fecha = input("Fecha (AAAA-MM-DD): ")
            hora = input("Hora (HH:MM): ")

            datetime.strptime(f"{fecha} {hora}", "%Y-%m-%d %H:%M")

            paciente = Paciente(cedula, nombre, especialidad, fecha, hora)
            if self.agenda.registrar_turno(paciente):
                print(f"{Fore.GREEN}Turno registrado correctamente.")
            else:
                print(f"{Fore.YELLOW}Ya existe un turno a esa hora.")
        except ValueError:
            print(f"{Fore.RED}Fecha u hora inválidas.")

    def consultar_por_cedula(self):
        cedula = input("Cédula del paciente: ")
        encontrados = self.agenda.buscar_por_cedula(cedula)
        if encontrados:
            print(f"{Fore.BLUE}🔍 Turnos encontrados:")
            for p in encontrados:
                print(p)
        else:
            print(f"{Fore.YELLOW}No se encontraron turnos.")

    def consultar_por_fecha(self):
        fecha = input("Ingrese la fecha (AAAA-MM-DD): ")
        try:
            datetime.strptime(fecha, "%Y-%m-%d")
            turnos = self.agenda.turnos_por_fecha(fecha)
            if turnos:
                print(f"{Fore.BLUE}Turnos del {fecha}:")
                for p in turnos:
                    print(p)
            else:
                print(f"{Fore.YELLOW}No hay turnos para esa fecha.")
        except ValueError:
            print(f"{Fore.RED}Fecha inválida.")

    def cancelar(self):
        cedula = input("Cédula del paciente para cancelar turno: ")
        if self.agenda.cancelar_turno(cedula):
            print(f"{Fore.GREEN}Turno cancelado correctamente.")
        else:
            print(f"{Fore.RED}No se encontró ese turno.")

    def reporte(self):
        print(f"{Fore.CYAN}📊 Reporte por especialidad:")
        for esp, count in self.agenda.reporte_por_especialidad().items():
            print(f"- {esp}: {count} turno(s)")

if __name__ == "__main__":
    InterfazConsola().mostrar_menu()
