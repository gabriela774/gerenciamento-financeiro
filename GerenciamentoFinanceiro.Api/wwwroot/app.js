const moeda = new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" });

const nomesMeses = ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
  "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"];

let anoAtual;
let mesAtual;

async function carregarSaldo() {
  const resposta = await fetch("/api/Resumo/saldo");
  if (!resposta.ok) throw new Error("Erro ao buscar o saldo");
  const dados = await resposta.json();

  document.getElementById("saldo").textContent = moeda.format(dados.saldo);
}

async function carregarMes(ano, mes) {
  const parametros = ano && mes ? `?ano=${ano}&mes=${mes}` : "";
  const resposta = await fetch(`/api/Resumo/mensal${parametros}`);
  if (!resposta.ok) throw new Error("Erro ao buscar o resumo do mês");
  const dados = await resposta.json();

  anoAtual = dados.ano;
  mesAtual = dados.mes;

  document.getElementById("mes-titulo").textContent = `${nomesMeses[dados.mes - 1]} de ${dados.ano}`;
  document.getElementById("entradas").textContent = moeda.format(dados.totalEntradas);
  document.getElementById("saidas").textContent = moeda.format(dados.totalSaidas);

  const resultado = document.getElementById("resultado");
  resultado.textContent = moeda.format(dados.resultado);
  resultado.className = dados.situacao;
}

async function carregarCategorias() {
  const resposta = await fetch("/api/Categorias");
  if (!resposta.ok) throw new Error("Erro ao buscar as categorias");
  const categorias = await resposta.json();

  const campo = document.getElementById("categoria");
  categorias
    .sort((a, b) => a.nome.localeCompare(b.nome, "pt-BR"))
    .forEach(categoria => {
      const opcao = document.createElement("option");
      opcao.value = categoria.id;
      opcao.textContent = categoria.nome;
      campo.appendChild(opcao);
    });
}

function celula(texto, classe) {
  const td = document.createElement("td");
  td.textContent = texto;
  if (classe) td.className = classe;
  return td;
}

async function carregarLancamentos() {
  const resposta = await fetch("/api/Lancamentos");
  if (!resposta.ok) throw new Error("Erro ao buscar os lançamentos");
  const todos = await resposta.json();

  const prefixo = `${anoAtual}-${String(mesAtual).padStart(2, "0")}`;
  const doMes = todos.filter(l => l.data.startsWith(prefixo));

  const lista = document.getElementById("lista");
  lista.innerHTML = "";

  if (doMes.length === 0) {
    lista.innerHTML = '<tr><td colspan="5">Nenhum lançamento neste mês.</td></tr>';
    return;
  }

  doMes.forEach(l => {
    const [ano, mes, dia] = l.data.split("-");
    const entrada = l.tipo === "entrada";

    const linha = document.createElement("tr");
    linha.appendChild(celula(`${dia}/${mes}/${ano}`));
    linha.appendChild(celula(l.categoria ? l.categoria.nome : ""));
    linha.appendChild(celula(entrada ? "Entrada" : "Saída", entrada ? "tipo-entrada" : "tipo-saida"));
    linha.appendChild(celula(moeda.format(l.valor), "num"));
    linha.appendChild(celula(l.observacao || ""));
    lista.appendChild(linha);
  });
}

async function atualizarTela() {
  await carregarSaldo();
  await carregarMes(anoAtual, mesAtual);
  await carregarLancamentos();
}

async function lerErro(resposta) {
  const texto = await resposta.text();
  try {
    const json = JSON.parse(texto);
    if (json.errors) return Object.values(json.errors).flat().join(" ");
  } catch {}
  return texto || "Erro inesperado.";
}

document.getElementById("form-lancamento").addEventListener("submit", async (evento) => {
  evento.preventDefault();

  const lancamento = {
    data: document.getElementById("data").value,
    tipo: document.getElementById("tipo").value,
    categoriaId: document.getElementById("categoria").value,
    valor: parseFloat(document.getElementById("valor").value),
    observacao: document.getElementById("observacao").value || null
  };

  const resposta = await fetch("/api/Lancamentos", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(lancamento)
  });

  if (!resposta.ok) {
    alert("Não foi possível lançar: " + await lerErro(resposta));
    return;
  }

  document.getElementById("valor").value = "";
  document.getElementById("observacao").value = "";
  document.getElementById("categoria").value = "";

  await atualizarTela();
});
async function mudarMes(delta) {
  let ano = anoAtual;
  let mes = mesAtual + delta;

  if (mes < 1) { mes = 12; ano--; }
  if (mes > 12) { mes = 1; ano++; }

  try {
    await carregarMes(ano, mes);
    await carregarLancamentos();
  } catch (erro) {
    console.error(erro);
  }
}

document.getElementById("mes-anterior").addEventListener("click", () => mudarMes(-1));
document.getElementById("mes-proximo").addEventListener("click", () => mudarMes(1));
async function iniciar() {
  try {
    document.getElementById("data").value = new Date().toLocaleDateString("sv-SE");
    await carregarCategorias();
    await carregarMes();
    await atualizarTela();
  } catch (erro) {
    console.error(erro);
  }
}

iniciar();