#include <iostream>

using namespace std;

int main(int, char **)
{
    int nlinhas;
    int ncol;
    double qtdObstaculos;

    cout<<"Numero de linhas: "; cin>>nlinhas;
    cout<<"Numero de colunas: "; cin>>ncol;
    cout<<"Porcentagem de obstaculos: "; cin>>qtdObstaculos;

    int qtNode = nlinhas*ncol;
    int test = qtNode * (qtdObstaculos/100);
    cout<<test<<endl;

    int **mat = new int*[nlinhas];

    for(int i = 0;i < nlinhas; ++i)
        mat[i] = new int[ncol];

    for(int i = 0;i < nlinhas; ++i){
        for(int j = 0;j < ncol; ++j){
            mat[0][0] = 11;
            mat[i][j] = rand() % 2;
            mat[nlinhas-1][ncol-2] = 12;
        }
    }

    int mapCounter = 0;
    int cont = 0;

    cout<<"Globals.matrix = new char[][] {  new char[] {";

        for(int i = 0; i < nlinhas; i++){
            for(int j = 0; j < ncol; j++){
                if(mapCounter == ncol){
                    cout<<"},"<<endl;
                    mapCounter = 0;
                    cout<<"new char[] {";
                }
                if(mat[i][j] == 12){

                    if(mapCounter == ncol-1){
                        cout<<"'E'";
                    } else {
                        cout<<"'E', ";
                    }
                }else if(mat[i][j] == 11){
                    if(mapCounter == ncol-1){
                        cout<<"'S'";
                    } else {
                        cout<<"'S', ";
                    }
                }else if(mat[i][j] == 1){
                    if(cont >= test){
                        if(mapCounter == ncol-1){
                            cout<<"'-'";
                        } else {
                            cout<<"'-', ";
                        }
                    } else {
                        if(mapCounter == ncol-1){
                            cout<<"'X'";
                        } else {
                            cout<<"'X', ";
                        }
                        cont++;
                    }
                }else if(mat[i][j] != 1){
                    if(mapCounter == ncol-1){
                        cout<<"'-'";
                    } else {
                        cout<<"'-', ";
                    }
                }
                mapCounter++;
            }
        }
        cout<<"}};";
        cout<<endl;
        cout<<endl;

    //liberar memória
    for(int i = 0;i < nlinhas; ++i)
        delete []mat[i];

    delete []mat;

    return 0;
}
