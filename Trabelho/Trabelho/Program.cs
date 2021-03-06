﻿using System;

namespace Trabelho
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Sorteio de cartas

            string[] cartaJogo = new string[30], corCarta = new string[30], numCarta = new string[30], baralhoJogador = new string[30];
            string[] cor = { "Verde", "Amarelo", "Azul", "Vermelho" };
            string[] num = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "+2", "Bloqueio" };
            Random numero = new Random();
            for (int i = 0; i < 30; i++)
            {
                numCarta[i] = num[numero.Next(0, 12)];
                corCarta[i] = cor[numero.Next(0, 4)];
                cartaJogo[i] = numCarta[i] +"-"+ corCarta[i];//quase uma "gêmea" do baralhojogador, mas não possui as colocaões, pois ela é utilizada para indicar apenas a carta que está na mesa.
                baralhoJogador[i] = "\n" + i + "°:" + numCarta[i] + "-" + corCarta[i];//"baralhoJogador" já possui á "colocação" da carta, utilizada para o jogador indicar onde está a carta que ele quer jogar.
             }
            #endregion

            #region Separação das cartas do Jogador
            string cartaJogador = baralhoJogador[1]+ baralhoJogador[2]+ baralhoJogador[3]+ baralhoJogador[4]+ baralhoJogador[5];// não usei a carta baralhoJogador[0], porque senão iria aparecer "0°:1-verde", por exemplo.
            #endregion

            #region Carta da mesa
            string cartaDaMesa = cartaJogo[0];// A carta da mesa não pode iniciar com bloqueio ou +2 (necessário solucionar isso na programação)
            Console.WriteLine("\n\nA carta da mesa é a: {0}", cartaDaMesa);
            #endregion

            #region Início do jogo

            for (int cont = 5; cont < 50;)// utilizado para por reiniciar a programação para uma nova jogada, tabmbém usado no sistema de compra de novas cartas. 
            {
                #region -Solicitação da Carta do Jogador

                int numeroEscolhido, k;
                do
                {
                    Console.WriteLine("Suas cartas São: {0}\n", cartaJogador);
                    k = 1;// usei o k como bode espiatório, se programa rodar normalmente k será igual a 1, se houver erros do usuário, k será igual a 0 e o while irá pedir uma carta válida
                    do
                    {
                    Console.WriteLine("\n\nQual a carta que você deseja jogar?");
                    numeroEscolhido = Convert.ToInt32(Console.ReadLine());// outra vantagem de ter descartado a variável baralhoJogador[0] é que posso usar o numeroEscolhido como referência de onde se encontra a carta jogada no baralho, vou explicar melhor mais pra frente ;)
                        #endregion

                #region -Processamento e Validação da carta escolhida pelo jogador

                        while (numeroEscolhido == 51)//caso o jogador não tenha nenhuma carta que possa ser jogada, ele poderá comprar +1 digitando "51".
                        {
                            cont++;//cont é tipo um "for" manual que adiciona um valor a ele mesmo quando o jogador pede uma carta nova, usando esse mesmo número para localizar a nova carta no baralho
                            cartaJogador += baralhoJogador[cont];
                            Console.WriteLine("\nSuas cartas São: {0}", cartaJogador);
                            Console.WriteLine("\n\nQual a carta que você deseja jogar?");
                            numeroEscolhido = Convert.ToInt32(Console.ReadLine());
                            
                        }
                            if (numeroEscolhido > cont && numeroEscolhido < 51 || numeroEscolhido > 51)// caso o jogador coloque qualquer número aleatório, o jogo irá recusar e pedir para o jogador jogar uma carta válida
                            {
                                Console.WriteLine("\nSelecione um número válido");
                            }
                    } while (numeroEscolhido > cont);// cont também é igual ao número de cartas na mão do jogador, se o numeroEscolhido for maior que o cont, o jogo irá pedir uma carta válida

                    //Aqui se encontra o processamento principal, que determina se a carta é verdadeiramente válida ou não
                    if (cartaDaMesa.Substring(2) == baralhoJogador[numeroEscolhido].Substring(6) ||// analisa a cor da "carta da mesa" e da "carta jogada", se ambas forem semelhantes a jogada é válida.   
                        cartaDaMesa.Substring(2) == baralhoJogador[numeroEscolhido].Substring(7) ||// mesma função da anterior porém analisa as cartas localizadas a partir da 10° posição, enquanto a outra analisa apenas cartas cuja "colocação" possui apenas um algarismo, ou seja, da 1° a 9° colocação.
                        cartaDaMesa.Substring(0, 1) == baralhoJogador[numeroEscolhido].Substring(4, 1) ||// analisa o número da "carta da mesa" e da "carta jogada", se ambos forem semelhantes a jogada é válida. 
                        cartaDaMesa.Substring(0, 1) == baralhoJogador[numeroEscolhido].Substring(5, 1))// mesma função da anterior porém analisa as cartas localizadas a partir da 10° posição, enquanto a outra analisa apenas cartas cuja "colocação" possui apenas um algarismo, ou seja, da 1° a 9° colocação.
                    {
                        cartaDaMesa = cartaJogo[numeroEscolhido];//substitui a carta da mesa pela a carta jogada
                        cartaJogador = cartaJogador.Replace(baralhoJogador[numeroEscolhido], "");//retira a carta jogada das cartas do jogador, porém ela continua no baralho disponível para ser jogada, este bug tem que ser solucionado. BUG 3
                    }
                    else if (cartaDaMesa.Substring(2) == baralhoJogador[numeroEscolhido].Substring(13) ||//analisa a cor da carta da mesa e a cor da carta de bloqueio jogada, se ambaas forem iguais, o jogador bloqueia o computador. BUG 4 (causa desconhecida)
                             cartaDaMesa.Substring(2) == baralhoJogador[numeroEscolhido].Substring(14))// mesma função da anterior porém analisa as cartas localizadas a partir da 10° posição, enquanto a outra analisa apenas cartas cuja "colocação" possui apenas um algarismo, ou seja, da 1° a 9° colocação.
                    {
                        cartaDaMesa = cartaJogo[numeroEscolhido].Replace("Bloqueio","X");//substitui a carta da mesa pela a carta jogada e substitui o nome bloqueio por um nome genérico "x", para ocupar o lugar do número na próxima jogada, ou seja, a próxima carta após o bloqueio deverá ser da mesma cor da carta do bloqueio.
                        cartaJogador = cartaJogador.Replace(baralhoJogador[numeroEscolhido], "");//retira a carta jogada das cartas do jogador, porém ela continua no baralho disponível para ser jogada, este bug tem que ser solucionado. BUG 3
                        k = 0;//com k = 0 o programa será reiniciado para que o jogador jogue mais uma vez, por ter bloqueado o oponente.
                        Console.WriteLine("Ieeeh, bloqueou o inimigo \nSelecione mais uma Carta!!");
                    }
                    else// se o cabeçudo do usuário selecionar uma carta de cor ou número errado, essa mensagem irá aparecer e a rodada será reiniciada.
                    {
                        k = 0;//olha o bode expiatório denovo ai minha gente
                        Console.WriteLine("Selecione uma carta válida.");
                    }
                } while (k==0);
                #endregion

                #region -Divulgação da carta jogada pelo usuário
                Console.WriteLine("\nVocê Jogou a Carta: {0}", cartaDaMesa);
                #endregion
                
                #region -Jogada do Computador
                do
                {
                    k = 1;// grande bode
                    // Basicamente o computador sorteia uma carta da mesma cor que a carta que o jogador selecionar, tem um jeito de simplificar essa programação, mas não estou feliz com essa parte. BUG 5
                    if (cartaDaMesa.Substring(2) == "Verde")
                    {
                        cartaDaMesa = num[numero.Next(0, 12)] + "-" + cor[0];
                    }
                    else if (cartaDaMesa.Substring(2) == "Amarelo")
                    {
                        cartaDaMesa = num[numero.Next(0, 12)] + "-" + cor[1];
                    }
                    else if (cartaDaMesa.Substring(2) == "Azul")
                    {
                        cartaDaMesa = num[numero.Next(0, 12)] + "-" + cor[2];
                    }
                    else if (cartaDaMesa.Substring(2) == "Vermelho")
                    {
                        cartaDaMesa = num[numero.Next(0, 12)] + "-" + cor[3];
                    }
                    
                    if (cartaDaMesa.Substring(0) == "Bloqueio-Verde" ||
                        cartaDaMesa.Substring(0) == "Bloqueio-Amarelo" ||
                        cartaDaMesa.Substring(0) == "Bloqueio-Azul" ||
                        cartaDaMesa.Substring(0) == "Bloqueio-Vermelho")
                    {
                        Console.WriteLine("Ops, o computador te bloqueou :(");
                        k = 0;
                    }

                } while (k == 0);
                #endregion

                #region -Divulgação da carta jogada pelo Computador
                Console.WriteLine("\\nComputer: {0}\n\n", cartaDaMesa);
                #endregion
            }
            #endregion

            Console.ReadKey();
        }
    }
}
