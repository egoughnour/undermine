CREATE KEYSPACE semantic WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : 1};
 USE semantic;
 CREATE TABLE terms (term_id int PRIMARY KEY, word text, document_id int, document_name text);
 INSERT INTO terms (term_id, word, document_id, document_name) VALUES (444, 'General', 2, 'frmMain.cs');
 INSERT INTO terms (term_id, word, document_id, document_name) VALUES (445, 'Matrix', 2, 'frmMain.cs');
 INSERT INTO terms (term_id, word, document_id, document_name) VALUES (446, 'Dot', 2, 'frmMain.cs');
 INSERT INTO terms (term_id, word, document_id, document_name) VALUES (447, 'Net', 2, 'frmMain.cs');
 INSERT INTO terms (term_id, word, document_id, document_name) VALUES (448, 'uk', 2, 'frmMain.cs');
 INSERT INTO terms (term_id, word, document_id, document_name) VALUES (449, 'u', 2, 'frmMain.cs');
 INSERT INTO terms (term_id, word, document_id, document_name) VALUES (450, 'Array', 2, 'frmMain.cs');
 INSERT INTO terms (term_id, word, document_id, document_name) VALUES (451, 'Length', 2, 'frmMain.cs');
 INSERT INTO terms (term_id, word, document_id, document_name) VALUES (452, 'svd_rank', 2, 'frmMain.cs');
 CREATE INDEX ON terms (document_name);






