#region USINGS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Z.MVVMHelper.Commands;
using Z.MVVMHelper.Interfaces;

#endregion

namespace Z.MVVMHelper
{
    /// <inheritdoc />
    /// <summary>
    ///     Group commands in VMs
    /// </summary>
    public sealed class CommandRepository : ICommandRepository
    {
        private readonly Dictionary<string, CommandBase?> _commands;

        /// <summary>
        ///     Default constructor for command repositories
        /// </summary>
        public CommandRepository() {
            _commands = new Dictionary<string, CommandBase?>();
        }

        /// <inheritdoc />
        
        public CommandBase? this[string key] { get => _commands[key]; set => _commands[key] = value; }

        /// <inheritdoc />
        public ICollection<string> Keys => _commands.Keys;

        /// <inheritdoc />
        public ICollection<CommandBase?> Values => _commands.Values;

        /// <inheritdoc />
        public int Count => _commands.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public void Add(string key, CommandBase? value) {
            _commands.Add(key, value);
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<string, CommandBase?> item) {
            ((IDictionary<string, CommandBase?>) _commands).Add(item);
        }

        /// <inheritdoc />
        public void AssignExceptionHandler(IExceptionHandler handler) {
            foreach (var command in Values) {
                if(command is null) continue;
                command.ExceptionHandler = handler;
            }
        }

        /// <inheritdoc />
        public void Clear() {
            _commands.Clear();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<string, CommandBase?> item) {
            return _commands.Contains(item);
        }

        /// <inheritdoc />
        public bool ContainsKey(string key) {
            return _commands.ContainsKey(key);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<string, CommandBase?>[] array, int arrayIndex) {
            ((IDictionary<string, CommandBase?>) _commands).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, CommandBase?>> GetEnumerator() {
            return _commands.GetEnumerator();
        }

        /// <inheritdoc />
        public bool Remove(string key) {
            return _commands.Remove(key);
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<string, CommandBase?> item) {
            return ((IDictionary<string, CommandBase?>) _commands).Remove(item);
        }

        /// <inheritdoc />
        public bool TryGetValue(string key, out CommandBase? value) {
            value = null;
            if (!_commands.TryGetValue(key, out var potentialValue))
                return false;

            value = potentialValue;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable) _commands).GetEnumerator();
        }
    }
}