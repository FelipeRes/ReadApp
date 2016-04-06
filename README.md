# ReadApp
Projeto Aplicativo de Acompanhamento de Leitura ReadApp. Desenvolvido por Felipe Resende no Instituto Federal do Piauí.
Features:

1.0	– Cadastrar livro por: Nome, Autor, Quantidade de Páginas e  Capítulos.

2.0	– Função de editar livro: Adicionar capitulo mais titulo, adicionar nota na página.

3.0	 - Registrar inicio e termino de leitura.

4.0	– Biblioteca do leitor com as três prateleiras: Quero Ler, Lendo, Lido.

5.0	– Classificar livros por gênero.

6.0	– Avaliação objetiva do livro: Rank por estrelas.


###Classes: 
**Livro**: Classe básica da aplicação que contem as informações essenciais ao livro, assim como uma lista de capítulos e gêneros. Ainda possui uma variável de avaliação que serve de nota para o livro.

**Leitura**: Classe que gerencia os estados da leitura, como ‘Iniciado’, ‘Terminado’. Ela ainda registra a página em que está o livro e a data do inicio e termino da leitura. Toda leitura possui um comentário que serve de avaliação do livro, ele é feito após que a leitura termina.

**Capitulo**: Objeto referente a um livro que marca trechos de começo e fim do livro além poder comentar esses trechos.

**Data**: Classe criada só para agilizar o registro das datas.

**DatabaseDAO**: Toda a comunicação com o banco de dados se dá através das funções dessa classe. O certo seria fazer um DAO para cada entidade, mas não tive tempo para organizar isso :p

**GeneroExtend** : Até onde sei, não é possível fazer estáticos métodos dentro de enums em C#, essa classe apenas serve para alocar a função estática que retornar um enum de gênero a partir de uma string com o nome do mesmo. Ele está no mesmo arquivo do enum original.

###Enums:
**EstadoLeitura**: Espera, Iniciado, Terminado. O estado espera só é usado caso a implementação não leve em conta a classe leitura.

**Genero**: Há vários gêneros enumerados aqui.

###Activity: 
**MainActivity**: É a atividade principal, divida em três abas(Lendo, Quero, Lido). Aqui é possível navegar pelas estantes e adicionar livros. Cada aba gera livros de acordo com o estado e mostra na tela informações mais relevantes naqueles estados.

**CapitulosActivity**: É a atividade que lista os capítulos dos livros e permite a adição de novos capítulos.

###Adapters:
**QueroLerAdapter**: Mostra informações básicas dos livros como seu nome e quantidade de páginas.

**LendoAdapter**: Esse adapter possui uma barra de progresso da leitura e um ícone à direita que ao ser tocado, abre uma janela onde é possível atualizar a página da leitura. Caso a página atual ultrapasse ou seja igual a quantidade de páginas do livro, o aplicativo entenderá que o você terminou a leitura e enviará o livro para a lista de livros Terminados.

**LidoAdapter**: Apresenta apenas o nome do livro, autor e avaliação objetiva em forma de estrelas.

**CapitulosAdapter**: Mostra os capítulos dos livros, enumerados e as páginas onde começam e terminam. 

###Fragments:
**QueroLer**: Funciona como uma activity mas é apenas uma das abas da MainActivity. Aqui é possível tocar nos livros e caso uma leitura ainda não tenha sido iniciada, é possível iniciá-la. Apagar um livro dessa seção irá apagar sua leitura respectiva, mas apagar a leitura não apaga o livro.

**Lendo**: Fragment que mostra os livros que estão no estado de Iniciado e suas barras de progresso.

**Lindo**: Fragment que mostra os livros que estão no estado de Terminado e sua avaliação. Ao tocar nos livros é possível ver todas as informações e ainda avaliar a leitura.

