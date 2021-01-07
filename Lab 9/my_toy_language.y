%{
#include <stdio.h>
#include <stdlib.h>
#define YYDEBUG 1

#define TIP_INT 1
#define TIP_CAR 2
#define TIP_STRING 3

double stiva[20];
int sp;

void push(double x)
{ stiva[sp++]=x; }

double pop()
{ return stiva[--sp]; }

%}

%union {
  	int l_val;
	char *p_val;
}

%token ELSE
%token IF
%token OUTPUT
%token INPUT
%token VAR
%token FOR

%token CHAR
%token NUMBER
%token STRING

%token TYPE
%token ARRAYTYPE
%token NE
%token LE
%token GE
%token EQ
%token LT
%token GT

%token ID
%token <p_val> CONST_INT
%token <p_val> CONST_CHAR
%token <p_val> CONST_STRING

%token ATRIB

%left '+' '-'
%left DIV MOD '*' '/'
%left OR
%left AND

%type <l_val> constantaNum
%type <p_val> constantaSir
%%

cmpdStmt: '{' stmtlist '}'
    ;

program: cmpdStmt
    ;

stmtlist: stmt 
    | stmt stmtlist
    ;

structStmt: ifStmt 
    | forStmt 
    | cmpdStmt
    ;

stmt: simplStmt ';' 
    | declList ':'
    | structStmt
    ;

ioStmt: INPUT 
    | OUTPUT ID
    | OUTPUT constanta
    ;

simplStmt: assignStmt 
    | ioStmt
    ;

assignStmt: ID '=' expression

expression: expression '+' term
    | expression '-' term 
    | term
    ;

term: term '*'  factor
    | term '/' factor 
    | term '%' factor 
    | factor
    ;

constanta: constantaNum
    | constantaSir
    ;

factor: '(' expression ')' 
    | ID 
    | constanta
    ;

declList:  VAR declaration lista_var
    | VAR declareAndAsignStmt lista_var
        ;

lista_var: ',' declaration 
    | ',' declareAndAsignStmt
    ;

declareAndAsignStmt: declaration "=" expression
    ;

declaration: ID ':' type
    ;

type: typeSimple 
    | arrayDecl
    ;

typeSimple: STRING
    | CHAR
    | NUMBER
    ;

constantaNum:	CONST_INT	{
			$$=TIP_INT;
			push(atof($1));
				}
		;

arrayDecl: typeSimple '.' '.' '.' '(' constantaNum ')'
    ;

constantaSir: CONST_CHAR	{
			$$ = TIP_CAR;
			push((char)$1[1]);
				}
    | CONST_STRING	{
			$$ = TIP_STRING;
			push($1);
				}
		;

ifStmt:  IF '(' condition ')' stmt ELSE stmt
    |   IF '(' condition ')' stmt ELSE cmpdStmt
    |   IF '(' condition ')' cmpdStmt ELSE stmt
    |   IF '(' condition ')' cmpdStmt ELSE cmpdStmt
    ;

forStmt: FOR '(' assignStmt condition ';' assignStmt ')' stmt
    |   FOR '(' assignStmt condition ';' assignStmt ')' cmpdStmt
    |   FOR '(' declList condition ';' assignStmt ')' stmt
    |   FOR '(' declList condition ';' assignStmt ')' cmpdStmt
    ;

condition: expression RELATION expression
    ;

RELATION: LT 
    | LE 
    | EQ 
    | NE
    | GE 
    | GT 
    | AND 
    | OR
    ;

%%

yyerror(char *s)
{
  printf("%s\n", s);
}

extern FILE *yyin;

main(int argc, char **argv)
{
  if(argc>1) yyin = fopen(argv[1], "r");
  if((argc>2)&&(!strcmp(argv[2],"-d"))) yydebug = 1;
  if(!yyparse()) fprintf(stderr,"\tO.K.\n");
}
