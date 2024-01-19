using DCMLockerServidor.Shared;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace DCMLockerServidor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresasOldController : ControllerBase
    {
        //Empresas
        [HttpGet]
        public IActionResult GetEmpresas()
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataEmpresas.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    return Ok(content);
                }
                else
                {
                    List<EmpresaOld> emptyList = new List<EmpresaOld>();
                    string emptyListJson = JsonSerializer.Serialize<List<EmpresaOld>>(emptyList);
                    return Ok(emptyListJson); // devolver lista vacia si el archivo no existe.
                }
            }
            catch
            {
                return StatusCode(500); // En caso de un error, devolver un código de estado 500 (Internal Server Error).
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEmpresaById(int id)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataEmpresas.ans");

                if (System.IO.File.Exists(sf))
                {

                    string content = System.IO.File.ReadAllText(sf);
                    var response = JsonSerializer.Deserialize<List<EmpresaOld>>(content);
                    return Ok(response.Where(x => x.Id == id).First());
                }
                else
                {
                    List<EmpresaOld> emptyList = new List<EmpresaOld>();
                    string emptyListJson = JsonSerializer.Serialize<List<EmpresaOld>>(emptyList);
                    return Ok(emptyListJson); // devolver lista vacia si el archivo no existe.
                }
            }
            catch
            {
                return StatusCode(500); // En caso de un error, devolver un código de estado 500 (Internal Server Error).
            }
        }


        [HttpPost("addEmpresa")]
        public bool AgregarEmpresa([FromBody] EmpresaOld empresa)
        {
            empresa.Id = 1;
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataEmpresas.ans");
                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<EmpresaOld> listaDeEmpresas = JsonSerializer.Deserialize<List<EmpresaOld>>(content);
                    if (listaDeEmpresas.Count > 0)
                    {
                        empresa.Id = listaDeEmpresas.Max(t => t.Id) + 1;
                    }
                    listaDeEmpresas.Add(empresa);

                    string s = JsonSerializer.Serialize<List<EmpresaOld>>(listaDeEmpresas);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {

                    List<EmpresaOld> listaDeEmpresas = new List<EmpresaOld> { empresa };
                    string s = JsonSerializer.Serialize<List<EmpresaOld>>(listaDeEmpresas);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }
        
       
        [HttpPost("editEmpresa")]
        public bool EditEmpresa([FromBody] EmpresaOld empresa)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataEmpresas.ans");
                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<EmpresaOld> listaDeEmpresas = JsonSerializer.Deserialize<List<EmpresaOld>>(content);

                    listaDeEmpresas = listaDeEmpresas.Where(x => x.Id != empresa.Id).ToList();
                    listaDeEmpresas.Add(empresa);

                    string s = JsonSerializer.Serialize<List<EmpresaOld>>(listaDeEmpresas);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                throw;
            }
        }

        [HttpPost("deleteEmpresa")]
        public bool DeleteEmpresa([FromBody] EmpresaOld empresa)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataEmpresas.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<EmpresaOld> listaDeEmpresas = JsonSerializer.Deserialize<List<EmpresaOld>>(content);

                    listaDeEmpresas = listaDeEmpresas.Where(x => x.Id != empresa.Id).ToList();

                    string s = JsonSerializer.Serialize<List<EmpresaOld>>(listaDeEmpresas);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                throw;
            }
        }

        //Locker a empresas
        [HttpGet("LockersDeEmpresas")]
        public IActionResult GetLockersDeEmpresas()
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataLockerEmpresa.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    return Ok(content);
                }
                else
                {
                    Dictionary<int, List<string>> emptyDictionary = new Dictionary<int, List<string>>();
                    string emptyDictionaryJson = JsonSerializer.Serialize<Dictionary<int, List<string>>>(emptyDictionary);
                    return Ok(emptyDictionaryJson); // devolver diccionario vacio si el archivo no existe.
                }
            }
            catch
            {
                return StatusCode(500); // En caso de un error, devolver un código de estado 500 (Internal Server Error).
            }
        }
        public class LockerEmpresa
        {
            public string NroSerieLocker { get; set; }
            public int IdEmpresa { get; set; }
        }
        [HttpPost("AddLockerAId")]
        public bool AgregarLockerAId([FromBody] LockerEmpresa lockEmpr)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataLockerEmpresa.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    Dictionary<int, List<string>> diccDeLockersAEmpresas = JsonSerializer.Deserialize<Dictionary<int, List<string>>>(content);

                    if (!diccDeLockersAEmpresas.ContainsKey(lockEmpr.IdEmpresa))
                    {
                        diccDeLockersAEmpresas[lockEmpr.IdEmpresa] = new List<string>();
                    }

                    diccDeLockersAEmpresas[lockEmpr.IdEmpresa].Add(lockEmpr.NroSerieLocker);


                    string s = JsonSerializer.Serialize(diccDeLockersAEmpresas);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    Dictionary<int, List<string>> diccDeLockersAEmpresas = new Dictionary<int, List<string>>();
                    diccDeLockersAEmpresas[lockEmpr.IdEmpresa] = new List<string>() { lockEmpr.NroSerieLocker };

                    string s = JsonSerializer.Serialize(diccDeLockersAEmpresas);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }
        

        [HttpPost("deleteLockerConIdEmpresa")]
        public bool DeleteLockerConIdEmpresa([FromBody] LockerEmpresa lockEmpr)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataLockerEmpresa.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    Dictionary<int, List<string>> diccDeLockersAEmpresas = JsonSerializer.Deserialize<Dictionary<int, List<string>>>(content);

                    if (diccDeLockersAEmpresas.ContainsKey(lockEmpr.IdEmpresa))
                    {
                        diccDeLockersAEmpresas[lockEmpr.IdEmpresa].Remove(lockEmpr.NroSerieLocker);
                    }
                    string s = JsonSerializer.Serialize(diccDeLockersAEmpresas);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                throw;
            }
        }

        [HttpPost("deleteLockerSinIdEmpresa")]
        public bool DeleteLockerSinIdEmpresa([FromBody] string nroSerieLocker)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataLockerEmpresa.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    Dictionary<int, List<string>> diccDeLockersAEmpresas = JsonSerializer.Deserialize<Dictionary<int, List<string>>>(content);

                    foreach (var kvp in diccDeLockersAEmpresas.ToList())
                    {
                        if (kvp.Value.Contains(nroSerieLocker))
                        {
                            kvp.Value.Remove(nroSerieLocker);
                        }
                        if (kvp.Value.Count == 0)
                        {
                            diccDeLockersAEmpresas.Remove(kvp.Key);
                        }
                    }

                    string s = JsonSerializer.Serialize(diccDeLockersAEmpresas);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                throw;
            }
        }

        //Locales
        [HttpGet("Locales")]
        public IActionResult GetLocales()
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataLocales.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    return Ok(content);
                }
                else
                {
                    List<Local> emptyList = new List<Local>();
                    string emptyListJson = JsonSerializer.Serialize<List<Local>>(emptyList);
                    return Ok(emptyListJson); // devolver lista vacia si el archivo no existe.
                }
            }
            catch
            {
                return StatusCode(500); // En caso de un error, devolver un código de estado 500 (Internal Server Error).
            }
        }

        [HttpPost("addLocales")]
        public bool AgregarLocal([FromBody] Local local)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataLocales.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<Local> listaDeLocales = JsonSerializer.Deserialize<List<Local>>(content);
                    if (listaDeLocales.Count > 0)
                    {
                        List<Local> listaDeLocalesPorEmpresa = listaDeLocales.Where(x => x.IdEmpresa == local.IdEmpresa).ToList();
                        if (listaDeLocalesPorEmpresa.Count > 0)
                        {
                            local.Id = listaDeLocalesPorEmpresa.Max(t => t.Id) + 1;
                        }
                        else
                        {
                            local.Id = 1;
                        }
                    }
                    else
                    {
                        local.Id = 1;
                    }
                    listaDeLocales.Add(local);

                    string s = JsonSerializer.Serialize<List<Local>>(listaDeLocales);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    local.Id = 1;
                    List<Local> listaDeLocales = new List<Local> { local };
                    string s = JsonSerializer.Serialize<List<Local>>(listaDeLocales);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("editLocal")]
        public bool EditLocal([FromBody] Local local)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataLocales.ans");
                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<Local> listaDeLocales = JsonSerializer.Deserialize<List<Local>>(content);

                    listaDeLocales = listaDeLocales.Where(x => x.Id != local.Id || x.IdEmpresa != local.IdEmpresa).ToList();
                    listaDeLocales.Add(local);

                    string s = JsonSerializer.Serialize<List<Local>>(listaDeLocales);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                throw;
            }
        }

        [HttpPost("deleteLocal")]
        public bool DeleteLocal([FromBody] Local local)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataLocales.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<Local> listaDeLocales = JsonSerializer.Deserialize<List<Local>>(content);

                    listaDeLocales = listaDeLocales.Where(x => x.Id != local.Id || x.IdEmpresa != local.IdEmpresa).ToList();

                    string s = JsonSerializer.Serialize<List<Local>>(listaDeLocales);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                throw;
            }
        }



    }
}
