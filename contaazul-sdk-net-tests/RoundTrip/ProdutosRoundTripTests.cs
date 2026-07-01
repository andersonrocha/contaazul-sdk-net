using ContaAzul.Sdk.Net.Models.Produtos;

namespace ContaAzul.Sdk.Net.Tests.RoundTrip;

[TestFixture]
public class ProdutosRoundTripTests
{
    // --- Listagem / detalhe ---

    [Test]
    public void ResumoDeProdutos_RoundTrips() => JsonRoundTrip.Verify<ResumoDeProdutos>(@"{
      ""items"": [
        {
          ""codigo"": ""PROD01"",
          ""contagem_agregacao"": 1,
          ""custo_medio"": 50,
          ""ean"": ""7891234567893"",
          ""estoque_maximo"": 1000,
          ""estoque_minimo"": 10,
          ""id"": ""0d4d34e3-5a1e-4d8b-8ed6-e0874ef1ccb8"",
          ""id_legado"": 37561,
          ""integracao_ecommerce_ativada"": false,
          ""movido"": true,
          ""nivel_estoque"": ""PADRAO"",
          ""nome"": ""Produto 01"",
          ""produtos_variacao"": [
            {
              ""aggregationCount"": 1,
              ""codigo"": ""PRODV01"",
              ""id"": ""4680e564-b995-4e05-8fec-b8f181497e93"",
              ""id_produto_pai_variacao"": ""9947a369-096e-4332-8e67-0a9765959e50"",
              ""movido"": false,
              ""nivel_estoque"": ""PADRAO"",
              ""nome"": ""Produto 01 var"",
              ""saldo"": 100,
              ""status"": ""ATIVO"",
              ""tipo"": ""PRODUTO"",
              ""valor_venda"": 150
            }
          ],
          ""saldo"": 1000,
          ""status"": ""ATIVO"",
          ""tipo"": ""PRODUTO"",
          ""ultima_atualizacao"": ""2025-07-10T15:05:08.433885Z"",
          ""valor_venda"": 150
        }
      ],
      ""totalItems"": 10
    }");

    [Test]
    public void Produto_RoundTrips() => JsonRoundTrip.Verify<Produto>(@"{
      ""ativo"": true,
      ""categoria"": { ""descricao"": ""Cat"", ""id"": 1, ""uuid"": ""9aa9af47-a539-40fc-9005-85c70e757fcf"" },
      ""codigo_ean"": ""EAN123"",
      ""codigo_sku"": ""SKU123"",
      ""conversao_unidade_medida"": [
        { ""fator"": 1000, ""id_fornecedor"": [""f-1""], ""id_unidade_conversao"": ""9aa9af47-a539-40fc-9005-85c70e757fcf"", ""unidade_medida"": { ""descricao"": ""UM"", ""id"": 1 } }
      ],
      ""descricao"": ""Descricao do produto"",
      ""detalhe_kit"": { ""items"": [ { ""codigo"": ""SKU123"", ""id_item"": ""i-1"", ""id_produto"": ""p-2"", ""nome"": ""item"", ""quantidade"": 2, ""valor_total"": 40, ""valor_unitario"": 20, ""versao"": 1 } ], ""valor_venda_kit"": 40 },
      ""ecommerce"": { ""categoria_ecommerce"": { ""descricao"": ""EcomCat"", ""id"": ""e-1"" }, ""condicao"": ""NOVO"", ""descricao_adicional"": ""adc"", ""integracao_ativa"": true, ""marca"": { ""id"": ""m-1"", ""nome"": ""Marca"" }, ""titulo_seo"": ""t"", ""url_seo"": ""https://x/y"" },
      ""estoque"": { ""custo_medio"": 10, ""maximumStock"": 100, ""minimumStock"": 10, ""quantidade_disponivel"": 50, ""quantidade_reservada"": 5, ""quantidade_total"": 55, ""valor_venda"": 150 },
      ""fiscal"": { ""cest"": { ""codigo"": ""1234"", ""descricao"": ""CEST"", ""id"": 1 }, ""ncm"": { ""codigo"": ""5678"", ""descricao"": ""NCM"", ""id"": 2 }, ""origem"": ""NACIONAL"", ""tipo_produto"": ""EMBALAGEM"", ""unidade_medida"": { ""descricao"": ""UN"", ""id"": 3 } },
      ""formato"": ""SIMPLES"",
      ""id"": ""9aa9af47-a539-40fc-9005-85c70e757fcf"",
      ""id_centro_custo"": ""cc-1"",
      ""id_legado"": 78305,
      ""imagens"": [ { ""descricao"": ""img"", ""id"": ""img-1"", ""nome"": ""imagem"", ""tamanho"": 1024, ""url_imagem"": ""https://example.com/image.jpg"" } ],
      ""nome"": ""nome do produto"",
      ""pesos_dimensoes"": { ""altura"": 10, ""largura"": 15, ""peso_bruto"": 1.5, ""peso_liquido"": 1, ""profundidade"": 5, ""volumes"": 10 },
      ""status"": ""ATIVO"",
      ""ultima_atualizacao"": ""2025-07-22T17:47:35.825004839Z"",
      ""unidade_medida"": { ""descricao"": ""UN"", ""id"": 3 },
      ""versao"": 1
    }");

    [Test]
    public void VariacaoDoProduto_RoundTrips() => JsonRoundTrip.Verify<VariacaoDoProduto>(@"{
      ""produtos"": [
        { ""codigo_ean"": ""EAN123"", ""codigo_sku"": ""SKU123"", ""id"": ""p-1"", ""movido"": false, ""nome"": ""pacote 5kg"", ""opcoes"": [ { ""descricao"": ""5kg"", ""id"": ""o-1"" } ], ""relacionado_manualmente"": true, ""saldo"": 10, ""valor_venda"": 100, ""versao"": 1 }
      ],
      ""tipos"": [
        { ""descricao"": ""pacote"", ""id"": ""t-1"", ""opcoes"": [ { ""descricao"": ""5kg"", ""id"": ""o-1"" } ] }
      ]
    }");

    // --- Listagens auxiliares ---

    [Test]
    public void CategoriasDeProduto_RoundTrips() => JsonRoundTrip.Verify<CategoriasDeProduto>(@"{
      ""items"": [ { ""descricao"": ""Eletronicos"", ""id"": 1, ""uuid"": ""550e8400-e29b-41d4-a716-446655440000"" } ],
      ""total_items"": 10
    }");

    [Test]
    public void CESTsDeProduto_RoundTrips() => JsonRoundTrip.Verify<CESTsDeProduto>(@"{
      ""items"": [ { ""codigo"": ""0100100"", ""descricao"": ""Molduras e acabamentos"", ""id"": 1 } ],
      ""total_items"": 10
    }");

    [Test]
    public void NCMsDeProduto_RoundTrips() => JsonRoundTrip.Verify<NCMsDeProduto>(@"{
      ""items"": [ { ""codigo"": ""0100400"", ""descricao"": ""Livros de registro"", ""id"": 1 } ],
      ""total_items"": 10
    }");

    [Test]
    public void UnidadesDeMedidaDeProduto_RoundTrips() => JsonRoundTrip.Verify<UnidadesDeMedidaDeProduto>(@"{
      ""items"": [ { ""abreviacao"": ""Kg"", ""descricao"": ""Kilograma"", ""em_uso"": true, ""id"": 10801 } ],
      ""total_items"": 10
    }");

    [Test]
    public void MarcaDeEcommerce_RoundTrips() => JsonRoundTrip.Verify<MarcaDeEcommerce>(@"{
      ""items"": [ { ""id"": ""0e9b442a-3af0-45f1-9c5c-415473084092"", ""nome"": ""adidas"" } ],
      ""total_items"": 10
    }");

    [Test]
    public void ProdutoEcommerceCategoria_RoundTrips() => JsonRoundTrip.Verify<ProdutoEcommerceCategoria>(@"{
      ""id"": ""b9ec512f-98a5-44b2-b65f-b378a1db3ce5"",
      ""items"": [ { ""descricao"": ""Eletronicos"", ""id"": ""847f77c6-8f70-466e-ae29-1bc91bca696e"", ""subcategorias"": [ { ""descricao"": ""Celulares"", ""id"": ""c-2"" } ] } ],
      ""versao"": 1
    }");

    // --- Criação / atualização parcial ---

    [Test]
    public void CriacaoProduto_RoundTrips() => JsonRoundTrip.Verify<CriacaoProduto>(@"{
      ""ativo"": true,
      ""categoria"": { ""id"": 1 },
      ""codigo_ean"": ""EAN123"",
      ""codigo_sku"": ""SKU123"",
      ""conversoes_unidade_medida"": [ { ""fator"": 1000, ""id_fornecedor"": [""f-1""], ""unidade_medida"": { ""id"": 1 } } ],
      ""descricao"": ""Descricao"",
      ""detalhe_kit"": { ""itens"": [ { ""id_produto"": ""p-2"", ""quantidade"": 1, ""valor_total"": 100, ""valor_unitario"": 100 } ], ""valor_venda"": 100 },
      ""ecommerce"": { ""categoria_ecommerce"": { ""id"": ""e-1"" }, ""condicao"": ""NOVO"", ""descricao_adicional"": ""adc"", ""descricao_seo"": ""seo"", ""integracao_ativa"": true, ""marca"": { ""id"": ""m-1"" }, ""titulo_seo"": ""t"", ""url_seo"": ""https://x/y"" },
      ""estoque"": { ""custo_medio"": 80, ""estoque_disponivel"": 50, ""estoque_maximo"": 100, ""estoque_minimo"": 10, ""valor_venda"": 100 },
      ""fiscal"": { ""cest"": { ""id"": 1 }, ""ncm"": { ""id"": 2 }, ""origem"": ""NACIONAL"", ""tipo_produto"": ""EMBALAGEM"", ""unidade_medida"": { ""id"": 3 } },
      ""formato"": ""VARIACAO"",
      ""id_centro_custo"": ""cc-1"",
      ""nome"": ""nome do produto"",
      ""pesos_dimensoes"": { ""altura"": 10, ""largura"": 10, ""peso_bruto"": 10, ""peso_liquido"": 10, ""profundidade"": 10, ""volumes"": 10 },
      ""status"": ""ATIVO"",
      ""unidade_medida"": { ""id"": 1 },
      ""variacao"": { ""produtos"": [ { ""codigo_ean"": ""EAN123"", ""codigo_sku"": ""SKU123"", ""nome"": ""pacote 5kg"", ""opcoes"": [ { ""id"": ""o-1"" } ], ""saldo"": 10, ""valor_venda"": 99.99 } ], ""tipos"": [ { ""descricao"": ""pacote"", ""opcoes"": [ { ""descricao"": ""5kg"", ""id"": ""o-1"" } ] } ] }
    }");

    [Test]
    public void AtualizacaoParcialProduto_RoundTrips() => JsonRoundTrip.Verify<AtualizacaoParcialProduto>(@"{
      ""cest"": 1,
      ""codigo_ean"": ""EAN123"",
      ""codigo_sku"": ""codigo do produto"",
      ""ncm"": 1,
      ""nome"": ""nome do produto"",
      ""peso_bruto"": 10,
      ""peso_liquido"": 10,
      ""unidade_medida"": 1,
      ""valor_venda"": 99.9
    }");
}
