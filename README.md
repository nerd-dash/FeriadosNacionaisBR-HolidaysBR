# Feriados Nacionais BR Holidays-BR
Lista com os feriados nacionais incluindo feriados correntes como Páscoa, Sexta-Feira Santa, Carnaval e Corpus Christi.

Como usar:

var listHolidaysBrazil = Utils.GetHolidaysByCurrentYear().ToList();

var dataTest = DateTime.Now; //Data que você deseja verificar se é feriado.

if(listHolidaysBrazil.Contains(dataTest))
{
//Entrou aqui, é feriado.
}

Caso deseje os feriados de um outro ano, é só passar por parâmetro:

var listHolidaysBrazil = Utils.GetHolidaysByCurrentYear(1992).ToList();
