/* Generated By:CSharpCC: Do not edit this line. PlSqlConstants.cs */
namespace Deveel.Data.Sql.Parser{
internal  class PlSqlConstants {

  public const int EOF = 0;
  public const int K_ALL = 5;
  public const int K_ALTER = 6;
  public const int K_AND = 7;
  public const int K_ANY = 8;
  public const int K_AS = 9;
  public const int K_ASC = 10;
  public const int K_BEGIN = 11;
  public const int K_BETWEEN = 12;
  public const int K_BINARY_INTEGER = 13;
  public const int K_BOOLEAN = 14;
  public const int K_BY = 15;
  public const int K_CALL = 16;
  public const int K_CASE = 17;
  public const int K_CAST = 18;
  public const int K_CHAR = 19;
  public const int K_CLOSE = 20;
  public const int K_COMMENT = 21;
  public const int K_COMMIT = 22;
  public const int K_COMMITTED = 23;
  public const int K_CONNECT = 24;
  public const int K_CONSTANT = 25;
  public const int K_CONSTRAINT = 26;
  public const int K_CONSTRAINTS = 27;
  public const int K_CURRENT = 28;
  public const int K_CURSOR = 29;
  public const int K_DATE = 30;
  public const int K_DECIMAL = 31;
  public const int K_DECLARE = 32;
  public const int K_DEFAULT = 33;
  public const int K_DELETE = 34;
  public const int K_DESC = 35;
  public const int K_DISTINCT = 36;
  public const int K_DO = 37;
  public const int K_ELSE = 38;
  public const int K_ELSIF = 39;
  public const int K_END = 40;
  public const int K_ESCAPE = 41;
  public const int K_EXCEPTION = 42;
  public const int K_EXCEPTION_INIT = 43;
  public const int K_EXCLUSIVE = 44;
  public const int K_EXISTS = 45;
  public const int K_EXIT = 46;
  public const int K_FETCH = 47;
  public const int K_FLOAT = 48;
  public const int K_FOR = 49;
  public const int K_FORALL = 50;
  public const int K_FROM = 51;
  public const int K_FULL = 52;
  public const int K_FUNCTION = 53;
  public const int K_GOTO = 54;
  public const int K_GROUP = 55;
  public const int K_HAVING = 56;
  public const int K_IF = 57;
  public const int K_IN = 58;
  public const int K_INDEX = 59;
  public const int K_INNER = 60;
  public const int K_INSERT = 61;
  public const int K_INTEGER = 62;
  public const int K_INTERSECT = 63;
  public const int K_INTO = 64;
  public const int K_IS = 65;
  public const int K_ISOLATION = 66;
  public const int K_LEFT = 67;
  public const int K_LIKE = 68;
  public const int K_LOCK = 69;
  public const int K_LOOP = 70;
  public const int K_MERGE = 71;
  public const int K_MINUS = 72;
  public const int K_NATURAL = 73;
  public const int K_NOT = 74;
  public const int K_NOWAIT = 75;
  public const int K_NULL = 76;
  public const int K_NULLS = 77;
  public const int K_NUMBER = 78;
  public const int K_OF = 79;
  public const int K_ON = 80;
  public const int K_ONLY = 81;
  public const int K_OPEN = 82;
  public const int K_OR = 83;
  public const int K_ORDER = 84;
  public const int K_OUT = 85;
  public const int K_OVER = 86;
  public const int K_PACKAGE = 87;
  public const int K_PARTITION = 88;
  public const int K_POSITIVE = 89;
  public const int K_PRAGMA = 90;
  public const int K_PRIOR = 91;
  public const int K_PROCEDURE = 92;
  public const int K_PX_GRANULE = 93;
  public const int K_RAISE = 94;
  public const int K_RANGE = 95;
  public const int K_READ = 96;
  public const int K_REAL = 97;
  public const int K_RECORD = 98;
  public const int K_REF = 99;
  public const int K_RETURN = 100;
  public const int K_RETURNING = 101;
  public const int K_REVERSE = 102;
  public const int K_RIGHT = 103;
  public const int K_ROLLBACK = 104;
  public const int K_ROW = 105;
  public const int K_ROWS = 106;
  public const int K_SAMPLE = 107;
  public const int K_SAVEPOINT = 108;
  public const int K_SELECT = 109;
  public const int K_SERIALIZABLE = 110;
  public const int K_SET = 111;
  public const int K_SHARE = 112;
  public const int K_SIBLINGS = 113;
  public const int K_SKIP = 114;
  public const int K_SMALLINT = 115;
  public const int K_SQL = 116;
  public const int K_START = 117;
  public const int K_TABLE = 118;
  public const int K_TEST = 119;
  public const int K_THEN = 120;
  public const int K_TO = 121;
  public const int K_TRANSACTION = 122;
  public const int K_UNION = 123;
  public const int K_UNIQUE = 124;
  public const int K_UPDATE = 125;
  public const int K_USE = 126;
  public const int K_USING = 127;
  public const int K_VALUES = 128;
  public const int K_VARCHAR2 = 129;
  public const int K_VARCHAR = 130;
  public const int K_WAIT = 131;
  public const int K_WHEN = 132;
  public const int K_WHERE = 133;
  public const int K_WHILE = 134;
  public const int K_WITH = 135;
  public const int K_WORK = 136;
  public const int K_WRITE = 137;
  public const int S_NUMBER = 138;
  public const int FLOAT = 139;
  public const int INTEGER = 140;
  public const int DIGIT = 141;
  public const int LINE_COMMENT = 142;
  public const int MULTI_LINE_COMMENT = 143;
  public const int S_IDENTIFIER = 144;
  public const int LETTER = 145;
  public const int SPECIAL_CHARS = 146;
  public const int S_BIND = 147;
  public const int S_CHAR_LITERAL = 148;
  public const int S_QUOTED_IDENTIFIER = 149;

  public const int DEFAULT = 0;

  public readonly string[] tokenImage = {
    "<EOF>",
    "\" \"",
    "\"\\t\"",
    "\"\\r\"",
    "\"\\n\"",
    "\"ALL\"",
    "\"ALTER\"",
    "\"AND\"",
    "\"ANY\"",
    "\"AS\"",
    "\"ASC\"",
    "\"BEGIN\"",
    "\"BETWEEN\"",
    "\"BINARY_INTEGER\"",
    "\"BOOLEAN\"",
    "\"BY\"",
    "\"CALL\"",
    "\"CASE\"",
    "\"CAST\"",
    "\"CHAR\"",
    "\"CLOSE\"",
    "\"COMMENT\"",
    "\"COMMIT\"",
    "\"COMMITTED\"",
    "\"CONNECT\"",
    "\"CONSTANT\"",
    "\"CONSTRAINT\"",
    "\"CONSTRAINTS\"",
    "\"CURRENT\"",
    "\"CURSOR\"",
    "\"DATE\"",
    "\"DECIMAL\"",
    "\"DECLARE\"",
    "\"DEFAULT\"",
    "\"DELETE\"",
    "\"DESC\"",
    "\"DISTINCT\"",
    "\"DO\"",
    "\"ELSE\"",
    "\"ELSIF\"",
    "\"END\"",
    "\"ESCAPE\"",
    "\"EXCEPTION\"",
    "\"EXCEPTION_INIT\"",
    "\"EXCLUSIVE\"",
    "\"EXISTS\"",
    "\"EXIT\"",
    "\"FETCH\"",
    "\"FLOAT\"",
    "\"FOR\"",
    "\"FORALL\"",
    "\"FROM\"",
    "\"FULL\"",
    "\"FUNCTION\"",
    "\"GOTO\"",
    "\"GROUP\"",
    "\"HAVING\"",
    "\"IF\"",
    "\"IN\"",
    "\"INDEX\"",
    "\"INNER\"",
    "\"INSERT\"",
    "\"INTEGER\"",
    "\"INTERSECT\"",
    "\"INTO\"",
    "\"IS\"",
    "\"ISOLATION\"",
    "\"LEFT\"",
    "\"LIKE\"",
    "\"LOCK\"",
    "\"LOOP\"",
    "\"MERGE\"",
    "\"MINUS\"",
    "\"NATURAL\"",
    "\"NOT\"",
    "\"NOWAIT\"",
    "\"NULL\"",
    "\"NULLS\"",
    "\"NUMBER\"",
    "\"OF\"",
    "\"ON\"",
    "\"ONLY\"",
    "\"OPEN\"",
    "\"OR\"",
    "\"ORDER\"",
    "\"OUT\"",
    "\"OVER\"",
    "\"PACKAGE\"",
    "\"PARTITION\"",
    "\"POSITIVE\"",
    "\"PRAGMA\"",
    "\"PRIOR\"",
    "\"PROCEDURE\"",
    "\"PX_GRANULE\"",
    "\"RAISE\"",
    "\"RANGE\"",
    "\"READ\"",
    "\"REAL\"",
    "\"RECORD\"",
    "\"REF\"",
    "\"RETURN\"",
    "\"RETURNING\"",
    "\"REVERSE\"",
    "\"RIGHT\"",
    "\"ROLLBACK\"",
    "\"ROW\"",
    "\"ROWS\"",
    "\"SAMPLE\"",
    "\"SAVEPOINT\"",
    "\"SELECT\"",
    "\"SERIALIZABLE\"",
    "\"SET\"",
    "\"SHARE\"",
    "\"SIBLINGS\"",
    "\"SKIP\"",
    "\"SMALLINT\"",
    "\"SQL\"",
    "\"START\"",
    "\"TABLE\"",
    "\"TEST\"",
    "\"THEN\"",
    "\"TO\"",
    "\"TRANSACTION\"",
    "\"UNION\"",
    "\"UNIQUE\"",
    "\"UPDATE\"",
    "\"USE\"",
    "\"USING\"",
    "\"VALUES\"",
    "\"VARCHAR2\"",
    "\"VARCHAR\"",
    "\"WAIT\"",
    "\"WHEN\"",
    "\"WHERE\"",
    "\"WHILE\"",
    "\"WITH\"",
    "\"WORK\"",
    "\"WRITE\"",
    "<S_NUMBER>",
    "<FLOAT>",
    "<INTEGER>",
    "<DIGIT>",
    "<LINE_COMMENT>",
    "<MULTI_LINE_COMMENT>",
    "<S_IDENTIFIER>",
    "<LETTER>",
    "<SPECIAL_CHARS>",
    "<S_BIND>",
    "<S_CHAR_LITERAL>",
    "<S_QUOTED_IDENTIFIER>",
    "\":\"",
    "\".\"",
    "\"=\"",
    "\";\"",
    "\"(\"",
    "\")\"",
    "\",\"",
    "\":=\"",
    "\"%TYPE\"",
    "\"%ROWTYPE\"",
    "\"<<\"",
    "\">>\"",
    "\"..\"",
    "\"+\"",
    "\"-\"",
    "\"||\"",
    "\"*\"",
    "\"/\"",
    "\"**\"",
    "\"%\"",
    "\"!\"",
    "\"#\"",
    "\">\"",
    "\"<\"",
    "\"@\"",
    "\"=>\"",
    "\"EXCEPT\"",
    "\".*\"",
    "\"OUTER\"",
  };

}
}
