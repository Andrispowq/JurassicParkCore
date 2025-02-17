namespace JurassicPark.Core.Functional;

public class DatabaseError(string message) : Error(message);
public sealed class EntryAlreadyExistsError() : DatabaseError("Entry already exists");
public sealed class EntryNotFoundError() : DatabaseError("Entry not found");