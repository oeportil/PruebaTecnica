CREATE TABLE Users(
 id serial PRIMARY KEY  NOT NULL,
 nombres varchar(255)  NOT NULL,
 apellidos varchar(255)  NOT NULL,
 fecha_nacimiento TIMESTAMP  NOT NULL, 
 direccion varchar(500)  NOT NULL,
 password varchar(255)  NOT NULL,
 telefono char(15)  NOT NULL,
 email varchar(65)  NOT NULL,
 estado char(1)  NOT NULL,
 fechaCreacion TIMESTAMP NOT NULL, 
 fechaModificacion TIMESTAMP 
);

ALTER TABLE users
ADD CONSTRAINT chk_estado CHECK (estado IN ('A', 'I'));

--para la creacion de fechas
CREATE OR REPLACE FUNCTION set_fecha_creacion()
RETURNS TRIGGER AS $$
BEGIN
    NEW.fechaCreacion := CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER trg_set_fecha_creacion
BEFORE INSERT ON Users
FOR EACH ROW
EXECUTE FUNCTION set_fecha_creacion();



--para la actualizacion de fecha 
CREATE OR REPLACE FUNCTION set_fecha_modificacion()
RETURNS TRIGGER AS $$
BEGIN
    NEW.fechaModificacion := CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_set_fecha_modificacion
BEFORE UPDATE ON Users
FOR EACH ROW
EXECUTE FUNCTION set_fecha_modificacion();