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
    public class EmpresasController : ControllerBase
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
                    List<Empresa> emptyList = new List<Empresa>();
                    string emptyListJson = JsonSerializer.Serialize<List<Empresa>>(emptyList);
                    return Ok(emptyListJson); // devolver lista vacia si el archivo no existe.
                }
            }
            catch
            {
                return StatusCode(500); // En caso de un error, devolver un código de estado 500 (Internal Server Error).
            }
        }


        [HttpPost("addEmpresa")]
        public bool AgregarEmpresa([FromBody] Empresa empresa)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataEmpresas.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<Empresa> listaDeEmpresas = JsonSerializer.Deserialize<List<Empresa>>(content);
                    if (listaDeEmpresas.Count > 0)
                    {
                        empresa.Id = listaDeEmpresas.Max(t => t.Id) + 1;
                    }
                    else
                    {
                        empresa.Id = 1;
                    }
                    listaDeEmpresas.Add(empresa);

                    string s = JsonSerializer.Serialize<List<Empresa>>(listaDeEmpresas);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    List<Empresa> listaDeEmpresas = new List<Empresa> { empresa };
                    string s = JsonSerializer.Serialize<List<Empresa>>(listaDeEmpresas);

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
        public bool EditEmpresa([FromBody] Empresa empresa)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataEmpresas.ans");
                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<Empresa> listaDeEmpresas = JsonSerializer.Deserialize<List<Empresa>>(content);

                    listaDeEmpresas = listaDeEmpresas.Where(x => x.Id != empresa.Id).ToList();
                    listaDeEmpresas.Add(empresa);

                    string s = JsonSerializer.Serialize<List<Empresa>>(listaDeEmpresas);

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
        public bool DeleteEmpresa([FromBody] Empresa empresa)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataEmpresas.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<Empresa> listaDeEmpresas = JsonSerializer.Deserialize<List<Empresa>>(content);

                    listaDeEmpresas = listaDeEmpresas.Where(x => x.Id != empresa.Id).ToList();

                    string s = JsonSerializer.Serialize<List<Empresa>>(listaDeEmpresas);

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

        [HttpPost("AddLockerAId")]
        public bool AgregarLockerAId([FromBody] string nroSerieLocker, int idEmpresa)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataLockerEmpresa.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    Dictionary<int, List<string>> diccDeLockersAEmpresas = JsonSerializer.Deserialize<Dictionary<int, List<string>>>(content);

                    if (!diccDeLockersAEmpresas.ContainsKey(idEmpresa))
                    {
                        diccDeLockersAEmpresas[idEmpresa] = new List<string>();
                    }

                    diccDeLockersAEmpresas[idEmpresa].Add(nroSerieLocker);


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
                    diccDeLockersAEmpresas[idEmpresa] = new List<string>() { nroSerieLocker };

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
        public bool DeleteLockerConIdEmpresa([FromBody] string nroSerieLocker, int idEmpresa)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataLockerEmpresa.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    Dictionary<int, List<string>> diccDeLockersAEmpresas = JsonSerializer.Deserialize<Dictionary<int, List<string>>>(content);

                    if (diccDeLockersAEmpresas.ContainsKey(idEmpresa))
                    {
                        diccDeLockersAEmpresas[idEmpresa].Remove(nroSerieLocker);
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
